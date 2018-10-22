using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimCapi
{

    public delegate void ChangeDelegate(List<SimCapiValue> value_list);
    public delegate void ConfigChangeDelegate(SimCapiConfig config);
    public delegate void InitialSetupCompleteDelegate();
    public delegate void HandshakeDelegate(SimCapiHandshake handshake);
    public delegate void CheckStartDelegate(Message.CheckStartResponse checkStartResponse);
    public delegate void CheckCompleteDelegate(Message.CheckCompleteResponse checkCompleteResponse);
    public delegate void ResizeSuccessDelegate();

    public delegate void SetDataRequestSuccessDelegate(Message.SetDataResponse setDataResponse);
    public delegate void SetDataRequestErrorDelegate(Message.SetDataResponse setDataResponse);

    public delegate void GetDataRequestSuccessDelegate(Message.GetDataResponse getDataResponse);
    public delegate void GetDataRequestErrorDelegate(Message.GetDataResponse getDataResponse);

    public delegate void LocalDataChangedDelegate(Message.RegisteredLocalDataChanged registeredLocalDataChanged);

    public class LocalDataChangedKey
    {
        public LocalDataChangedDelegate localDataChangedDelegate;

        string _simId;
        string _key;

        public string simId { get { return _simId; } }
        public string key { get { return _key; } }

        public LocalDataChangedKey(LocalDataChangedDelegate localDataChangedDelegate, string simId, string key)
        {
            this.localDataChangedDelegate = localDataChangedDelegate;
            _simId = simId;
            _key = key;
        }

        public void invalidateKey()
        {
            _simId = null;
            _key = null;
            localDataChangedDelegate = null;
        }

    }

    public class ChangeDelegateKey
    {
        public ChangeDelegate changeDelegate;

        public ChangeDelegateKey(ChangeDelegate changeDelegate)
        {
            this.changeDelegate = changeDelegate;
        }
    }

    public class ConfigChangeDelegateKey
    {
        public ConfigChangeDelegate configChangeDelegate;

        public ConfigChangeDelegateKey(ConfigChangeDelegate configChangeDelegate)
        {
            this.configChangeDelegate = configChangeDelegate;
        }
    }

    public class CheckCompleteKey
    {
        public CheckCompleteDelegate checkCompleteDelegate;
        public bool once;

        public CheckCompleteKey(CheckCompleteDelegate checkCompleteDelegate, bool once)
        {
            this.checkCompleteDelegate = checkCompleteDelegate;
            this.once = once;
        }
    }

    public class CheckStartKey
    {
        public CheckStartDelegate checkStartDelegate;
        public bool once;

        public CheckStartKey(CheckStartDelegate checkStartDelegate, bool once)
        {
            this.checkStartDelegate = checkStartDelegate;
            this.once = once;
        }
    }

    public class SimCapiSetRequestCallback
    {
        public SetDataRequestSuccessDelegate successDelegate;
        public SetDataRequestErrorDelegate errorDelegate;
        public SimCapiQueuedSetRequest queuedSetRequest;

        public SimCapiSetRequestCallback(SetDataRequestSuccessDelegate successDelegate, SetDataRequestErrorDelegate errorDelegate)
        {
            this.successDelegate = successDelegate;
            this.errorDelegate = errorDelegate;
        }
    }

    public class SimCapiQueuedSetRequest
    {
        public string value;
        public SetDataRequestSuccessDelegate successDelegate;
        public SetDataRequestErrorDelegate errorDelegate;

        public SimCapiQueuedSetRequest(string value, SetDataRequestSuccessDelegate successDelegate, SetDataRequestErrorDelegate errorDelegate)
        {
            this.value = value;
            this.successDelegate = successDelegate;
            this.errorDelegate = errorDelegate;
        }
    }

    public class SimCapiGetRequestCallback
    {
        public GetDataRequestSuccessDelegate successDelegate;
        public GetDataRequestErrorDelegate errorDelegate;
        public SimCapiQueuedGetRequest queuedGetRequest;

        public SimCapiGetRequestCallback(GetDataRequestSuccessDelegate successDelegate, GetDataRequestErrorDelegate errorDelegate)
        {
            this.successDelegate = successDelegate;
            this.errorDelegate = errorDelegate;
        }
    }


    public class SimCapiQueuedGetRequest
    {
        public GetDataRequestSuccessDelegate successDelegate;
        public GetDataRequestErrorDelegate errorDelegate;

        public SimCapiQueuedGetRequest(GetDataRequestSuccessDelegate successDelegate, GetDataRequestErrorDelegate errorDelegate)
        {
            this.successDelegate = successDelegate;
            this.errorDelegate = errorDelegate;
        }
    }


    public class Transporter
    {
        public static float valueChangeDelay = 0.025f;

        static Transporter _singleton;
        static GameObject _messageReceiver = null;

        bool _initialSetupComplete;
        bool _handshakeComplete;
        bool _pendingOnReady;
        bool _checkTriggered;

        int _resizeMessageId;

        SimCapiModel _simCapiModel;
        SimCapiLocalData _simCapiLocalData;

        SimCapiHandshake _handshake;

        List<string> _pendingMessagesForHandshake;
        List<string> _pendingMessagesForValueChange;

        Dictionary<string, SimCapiValue> _outGoingMap;

        HashSet<ChangeDelegateKey> _changeListeners;
        HashSet<ConfigChangeDelegateKey> _configChangeListeners;


        List<CheckCompleteKey> _checkCompleteListeners;
        List<CheckStartKey> _checkStartListeners;

        List<InitialSetupCompleteDelegate> _initialSetupCompleteListeners;
        List<HandshakeDelegate> _handshakeListeners;

        MessagePipe _messagePipe;

        Dictionary<string, Dictionary<string, SimCapiSetRequestCallback>> _setRequests;
        Dictionary<string, Dictionary<string, SimCapiGetRequestCallback>> _getRequests;

        Dictionary<int, ResizeSuccessDelegate> _resizeCallbacks;

        Dictionary<string, Dictionary<string, LocalDataChangedKey>> _localDataChangedCallbacks;

        SimCapiTimer _simCapiTimer;


        public static Transporter getInstance()
        {
            if (_singleton == null)
                _singleton = new Transporter(new IFramePipe());

            return _singleton;
        }

        public static void setSingleton(Transporter transporter)
        {
            _singleton = transporter;
        }


       
        public MessagePipe getMessagePipe()
        {
            return _messagePipe;
        }

        public SimCapiHandshake getHandshake()
        {
            return _handshake;
        }

        public SimCapiModel getModel()
        {
            return _simCapiModel;
        }

        static string generateName()
        {
            string uuid = "";
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            for (var i = 0; i < 10; i++)
            {
                uuid += chars[Random.Range((int)0, chars.Length)];
            }
            return uuid;
        }


        void createMessageReciverObject()
        {
            if (_messageReceiver != null)
                return;

            string receiverName = generateName();

            ExternalCalls.bindMessageListener(receiverName);

            _messageReceiver = new GameObject(receiverName);
            SimCapiMono simCapiMono = _messageReceiver.AddComponent<SimCapiMono>();
            simCapiMono.transporter = this;   
        }

        public Transporter(MessagePipe messagePipe)
        {
            createMessageReciverObject();

            _messagePipe = messagePipe;
            _messagePipe.transporter = this;

            _initialSetupComplete = false;
            _handshakeComplete = false;
            _pendingOnReady = false;
            _checkTriggered = false;

            _resizeMessageId = 0;

            _handshake = SimCapiHandshake.create(uuid.generate(), null, "<%= version %>");


            _pendingMessagesForHandshake = new List<string>();
            _pendingMessagesForValueChange = new List<string>();

            _outGoingMap = new Dictionary<string, SimCapiValue>();

            _changeListeners = new HashSet<ChangeDelegateKey>();
            _configChangeListeners = new HashSet<ConfigChangeDelegateKey>();

            _checkCompleteListeners = new List<CheckCompleteKey>();
            _checkStartListeners = new List<CheckStartKey>();

            _initialSetupCompleteListeners = new List<InitialSetupCompleteDelegate>();
            _handshakeListeners = new List<HandshakeDelegate>();

            _setRequests = new Dictionary<string, Dictionary<string, SimCapiSetRequestCallback>>();
            _getRequests = new Dictionary<string, Dictionary<string, SimCapiGetRequestCallback>>();

            _resizeCallbacks = new Dictionary<int, ResizeSuccessDelegate>();

            _localDataChangedCallbacks = new Dictionary<string, Dictionary<string, LocalDataChangedKey>>();


            _simCapiModel = new SimCapiModel(this);
            _simCapiLocalData = new SimCapiLocalData();

            // Value change delay
            _simCapiTimer = new SimCapiTimer(valueChangeDelay, sendValueChange);
        }

        public void update(float deltaTime)
        {
            _simCapiTimer.update(deltaTime);
        }

        /*
         * Send a HANDSHAKE_REQUEST message.
         */
        public void requestHandshake()
        {
            string jsonString = SimCapiJsonMaker.create_HANDSHAKE_REQUEST(_handshake);
            sendMessage(jsonString);
        }


        public void notifyOnReady()
        {
            if (_handshake.authToken == null)
            {
                _pendingOnReady = true;

                // once everything is ready, we request a handshake from the viewer.
                requestHandshake();
            }
            else
            {
                string onReadyJsonString = SimCapiJsonMaker.create_ON_READY(_handshake);

                sendMessage(onReadyJsonString);
                _pendingOnReady = false;

                foreach (HandshakeDelegate handshakeDelegate in _handshakeListeners)
                {
                    handshakeDelegate(_handshake);
                }

                _handshakeComplete = true;
                _handshakeListeners.Clear();

                // send initial value snapshot
                notifyValueChange();
            }

            if(!ExternalCalls.isInIFrame())
            {
                Message.InitialSetupComplete initialSetupComplete  = new Message.InitialSetupComplete();
                initialSetupComplete.handshake = _handshake;
                initialSetupComplete.handshake.config = new SimCapiConfig();
                initialSetupComplete.handshake.config.context = "VIEWER";

                handleInitialSetupComplete(initialSetupComplete);
            }
        }

        void notifyValueChange()
        {
            if (_handshake.authToken == null)
                return;

            if (_simCapiTimer.enabled() == false)
                _simCapiTimer.start();
        }


        void sendValueChange()
        {
            string jsonString = SimCapiJsonMaker.create_VALUE_CHANGE(_handshake, _outGoingMap);
            sendMessage(jsonString);

            foreach (string pendingMessage in _pendingMessagesForValueChange)
            {
                sendMessage(pendingMessage);
            }

            _pendingMessagesForValueChange.Clear();
        }


        public void sendMessage(string jsonString)
        {
            _messagePipe.sendMessage(jsonString);
        }

        public void reciveJsonMessage(string jsonString)
        {
            SimCapiMessageType messageType = SimCapiMessageType.ALLOW_INTERNAL_ACCESS;

            System.Object message = Message.deserialize(jsonString, ref messageType);

            if (message == null)
                return;

            handleMessage(message, messageType);
        }

        public void handleMessage(System.Object message, SimCapiMessageType messageType)
        {
            switch (messageType)
            {
                case SimCapiMessageType.HANDSHAKE_RESPONSE:
                    handleHandshakeResponse((Message.HandshakeResponse)message);
                    break;

                case SimCapiMessageType.VALUE_CHANGE:
                    handleValueChangeMessage((Message.ValueChange)message);
                    break;

                case SimCapiMessageType.CONFIG_CHANGE:
                    handleConfigChangeMessage((Message.ConfigChange)message);
                    break;

                case SimCapiMessageType.VALUE_CHANGE_REQUEST:
                    handleValueChangeRequestMessage((Message.ValueChangeRequest)message);
                    break;

                case SimCapiMessageType.CHECK_COMPLETE_RESPONSE:
                    handleCheckCompleteResponse((Message.CheckCompleteResponse)message);
                    break;

                case SimCapiMessageType.CHECK_START_RESPONSE:
                    handleCheckStartResponse((Message.CheckStartResponse)message);
                    break;

                case SimCapiMessageType.GET_DATA_RESPONSE:
                    handleGetDataResponse((Message.GetDataResponse)message);
                    break;

                case SimCapiMessageType.SET_DATA_RESPONSE:
                    handleSetDataResponse((Message.SetDataResponse)message);
                    break;

                case SimCapiMessageType.API_CALL_RESPONSE:
                    break;

                case SimCapiMessageType.INITIAL_SETUP_COMPLETE:
                    handleInitialSetupComplete((Message.InitialSetupComplete)message);
                    break;

                case SimCapiMessageType.RESIZE_PARENT_CONTAINER_RESPONSE:
                    handleResizeParentContainerResponse((Message.ResizeParentContainerResponse)message);
                    break;

                case SimCapiMessageType.ALLOW_INTERNAL_ACCESS:
                    setDomainToShortform();
                    break;

                case SimCapiMessageType.REGISTERED_LOCAL_DATA_CHANGED:
                    handleLocalDataChanged((Message.RegisteredLocalDataChanged)message);
                    break;
            }
        }





        #region ChangeListeners

        public void removeChangeListener(ChangeDelegateKey changeDelegateKey)
        {
            _changeListeners.Remove(changeDelegateKey);
        }

        public ChangeDelegateKey addChangeListener(ChangeDelegate changeDelegate)
        {
            ChangeDelegateKey changeDelegateKey = new ChangeDelegateKey(changeDelegate);
            _changeListeners.Add(changeDelegateKey);
            return changeDelegateKey;
        }

        public void removeAllChangeListeners()
        {
            _changeListeners.Clear();
        }

        public void removeConfigChangeListener(ConfigChangeDelegateKey configChangeDelegateKey)
        {
            _configChangeListeners.Remove(configChangeDelegateKey);
        }

        public ConfigChangeDelegateKey addConfigChangeListener(ConfigChangeDelegate configChangeDelegate)
        {
            ConfigChangeDelegateKey configChangeDelegateKey = new ConfigChangeDelegateKey(configChangeDelegate);
            _configChangeListeners.Add(configChangeDelegateKey);
            return configChangeDelegateKey;
        }

        public void removeAllConfigChangeListeners()
        {
            _configChangeListeners.Clear();
        }

        void callChangeListeners(List<SimCapiValue> values)
        {
            foreach (ChangeDelegateKey changeDelegateKey in _changeListeners)
            {
                changeDelegateKey.changeDelegate(values);
            }
        }

        void callConfigChangeListeners(SimCapiConfig config)
        {
            foreach (ConfigChangeDelegateKey configChangeDelegateKey in _configChangeListeners)
            {
                configChangeDelegateKey.configChangeDelegate(config);
            }
        }

        public void addInitialSetupCompleteListener(InitialSetupCompleteDelegate initialSetupCompleteDelegate)
        {
            if (_initialSetupComplete)
                throw new System.Exception("Initial setup already complete. This listener will never be called");

            _initialSetupCompleteListeners.Add(initialSetupCompleteDelegate);
        }

        public void removeAllInitialSetupCompleteListeners()
        {
            _initialSetupCompleteListeners.Clear();
        }

        public void addCheckCompleteListener(CheckCompleteDelegate checkCompleteDelegate, bool once)
        {
            _checkCompleteListeners.Add(new CheckCompleteKey(checkCompleteDelegate, once));
        }

        public void addCheckStartListener(CheckStartDelegate checkStartDelegate, bool once)
        {
            _checkStartListeners.Add(new CheckStartKey(checkStartDelegate, once));
        }

        public void addHandshakeCompleteListener(HandshakeDelegate handshakeDelegate)
        {
            if (_handshakeComplete)
            {
                handshakeDelegate(_handshake);
                return;
            }

            _handshakeListeners.Add(handshakeDelegate);
        }

        #endregion


        public SimCapiConfig getConfig()
        {
            return _handshake.config;
        }

        public void expose(SimCapiValue simCapiValue)
        {
            string key = simCapiValue.key;


            if (_outGoingMap.ContainsKey(key) && _outGoingMap[key].exposed == true)
                return;


            // Set the value to be exposed
            simCapiValue.exposed = true;

            // Override the value with the one already in the map
            if (_outGoingMap.ContainsKey(key) && _outGoingMap[key].exposed == false)
            {
                simCapiValue = _outGoingMap[key];
                List<SimCapiValue> changedList = new List<SimCapiValue>();
                changedList.Add(simCapiValue);
                callChangeListeners(changedList);
            }

            // add it to the map
            _outGoingMap[key] = simCapiValue;

            notifyValueChange();
        }

        public void removeValue(string key)
        {
            _outGoingMap.Remove(key);

            notifyValueChange();
        }

        #region MessageHandleFunctions
        void handleHandshakeResponse(Message.HandshakeResponse handshakeResponse)
        {
            if (handshakeResponse.handshake.requestToken != _handshake.requestToken)
                return;

            _handshake.authToken = handshakeResponse.handshake.authToken;
            _handshake.config = handshakeResponse.handshake.config;

            if (_pendingOnReady)
            {
                notifyOnReady();

                //trigger queue
                for (var i = 0; i < _pendingMessagesForHandshake.Count; ++i)
                {
                    sendMessage(_pendingMessagesForHandshake[i]);
                }
                _pendingMessagesForHandshake.Clear();
            }

            callConfigChangeListeners(_handshake.config);
        }

        void handleValueChangeMessage(Message.ValueChange valueChange)
        {
            if (valueChange.handshake.authToken != _handshake.authToken)
                return;

            List<SimCapiValue> changed_values = new List<SimCapiValue>();

            foreach (KeyValuePair<string, SimCapiValue> keyPair in valueChange.values)
            {
                string key = keyPair.Key;
                SimCapiValue simCapiValue = keyPair.Value;


                if (simCapiValue != null && !simCapiValue.isReadonly)
                {
                    bool exists_in_map = _outGoingMap.ContainsKey(key);

                    // Does the value exist in the map and are the values different?
                    if (exists_in_map == true && _outGoingMap[key].compare(simCapiValue) == false)
                    {
                        _outGoingMap[key].setValue(simCapiValue);

                        changed_values.Add(simCapiValue);
                    }
                    else if(exists_in_map == false)
                    {
                        simCapiValue.exposed = false;
                        _outGoingMap[key] = simCapiValue;

                        changed_values.Add(simCapiValue);
                    }
                }
            }

            if (changed_values.Count > 0)
                callChangeListeners(changed_values);
        }

        void handleConfigChangeMessage(Message.ConfigChange configChange)
        {
            if (configChange.handshake.authToken != _handshake.authToken)
                return;

            _handshake.config = configChange.handshake.config;
            callConfigChangeListeners(_handshake.config);
        }

        void handleValueChangeRequestMessage(Message.ValueChangeRequest valueChangeRequest)
        {
            if (valueChangeRequest.handshake.authToken != _handshake.authToken)
                return;

            notifyValueChange();
        }

        void handleCheckCompleteResponse(Message.CheckCompleteResponse checkCompleteResponse)
        {
            _checkTriggered = false;

            for (int i = 0; i < _checkCompleteListeners.Count; ++i)
            {
                CheckCompleteKey checkCompleteKey = _checkCompleteListeners[i];
                checkCompleteKey.checkCompleteDelegate(checkCompleteResponse);

                // Remove if once == true
                if (checkCompleteKey.once)
                {
                    _checkCompleteListeners.RemoveAt(i);
                    i--;
                }
            }
        }

        void handleCheckStartResponse(Message.CheckStartResponse checkStartResponse)
        {
            for (int i = 0; i < _checkStartListeners.Count; ++i)
            {
                CheckStartKey checkStartKey = _checkStartListeners[i];
                checkStartKey.checkStartDelegate(checkStartResponse);

                // Remove if once == true
                if (checkStartKey.once)
                {
                    _checkStartListeners.RemoveAt(i);
                    i--;
                }
            }
        }

        void handleGetDataResponse(Message.GetDataResponse getDataResponse)
        {
            if (getDataResponse.handshake.authToken != _handshake.authToken)
                return;

            SimCapiGetRequestCallback requestCallBack = _getRequests[getDataResponse.simId][getDataResponse.key];

            if (getDataResponse.responseType == "success" && requestCallBack.successDelegate != null)
                requestCallBack.successDelegate(getDataResponse);
            else if (getDataResponse.responseType == "error" && requestCallBack.errorDelegate != null)
                requestCallBack.errorDelegate(getDataResponse);

            SimCapiQueuedGetRequest queuedGetRequest = requestCallBack.queuedGetRequest;

            _getRequests[getDataResponse.simId].Remove(getDataResponse.key);

            if (queuedGetRequest != null)
                getDataRequest(getDataResponse.simId, getDataResponse.key, queuedGetRequest.successDelegate, queuedGetRequest.errorDelegate);
        }

        void handleSetDataResponse(Message.SetDataResponse setDataResponse)
        {
            if (setDataResponse.handshake.authToken != _handshake.authToken)
                return;

            SimCapiSetRequestCallback requestCallback = _setRequests[setDataResponse.simId][setDataResponse.key];

            if (setDataResponse.responseType == "success" && requestCallback.successDelegate != null)
                requestCallback.successDelegate(setDataResponse);
            else if (setDataResponse.responseType == "error" && requestCallback.errorDelegate != null)
                requestCallback.errorDelegate(setDataResponse);

            SimCapiQueuedSetRequest queuedSetRequest = requestCallback.queuedSetRequest;

            _setRequests[setDataResponse.simId].Remove(setDataResponse.key);

            if (queuedSetRequest != null)
                setDataRequest(setDataResponse.simId, setDataResponse.key, queuedSetRequest.value, queuedSetRequest.successDelegate, queuedSetRequest.errorDelegate);
        }

        void handleInitialSetupComplete(Message.InitialSetupComplete initialSetupComplete)
        {
            if (_initialSetupComplete || initialSetupComplete.handshake.authToken != _handshake.authToken)
                return;

            foreach (InitialSetupCompleteDelegate listener in _initialSetupCompleteListeners)
            {
                listener();
            }
            _initialSetupComplete = true;
        }

        void handleResizeParentContainerResponse(Message.ResizeParentContainerResponse resizeParentContainerResponse)
        {
            int messageId = resizeParentContainerResponse.messageId;

            if (_resizeCallbacks.ContainsKey(messageId) == false)
                return;

            if (resizeParentContainerResponse.responseType == "success")
                _resizeCallbacks[messageId]();

            _resizeCallbacks.Remove(resizeParentContainerResponse.messageId);
        }

        void handleLocalDataChanged(Message.RegisteredLocalDataChanged registeredLocalDataChanged)
        {
            _localDataChangedCallbacks[registeredLocalDataChanged.simId][registeredLocalDataChanged.key].localDataChangedDelegate(registeredLocalDataChanged);
        }

        #endregion
        public void triggerCheck(CheckCompleteDelegate checkCompleteDelegate)
        {
            if (_checkTriggered)
                throw new System.Exception("You have already triggered a check event");

            _checkTriggered = true;

            if (checkCompleteDelegate != null)
                addCheckCompleteListener(checkCompleteDelegate, true);

            string checkRequestMessage = SimCapiJsonMaker.create_CHECK_REQUEST(_handshake);
            _pendingMessagesForValueChange.Add(checkRequestMessage);

            notifyValueChange();
        }

        public void triggerCheck()
        {
            triggerCheck(null);
        }

        public bool setDataRequest(string simId, string key, string value, SetDataRequestSuccessDelegate successDelegate, SetDataRequestErrorDelegate errorDelegate)
        {
            if (simId == null)
                throw new System.Exception("setDataRequest() called with null simId!");

            if (key == null)
                throw new System.Exception("setDataRequest() called with null key!");

            if (value == null)
                throw new System.Exception("setDataRequest() called with null value!");

            // Use local data fallback
            if (!ExternalCalls.isInIFrame() || ExternalCalls.isInAuthor())
            {
                _simCapiLocalData.setData(simId, key, value, successDelegate);
                return true;
            }

            // Make sure dictionary exists for simId
            if (_setRequests.ContainsKey(simId) == false)
                _setRequests[simId] = new Dictionary<string, SimCapiSetRequestCallback>();

            // dataRequest in progress, add to queue
            if (_setRequests[simId].ContainsKey(key) == true)
            {
                _setRequests[simId][key].queuedSetRequest = new SimCapiQueuedSetRequest(value, successDelegate, errorDelegate);
                return false;
            }

            _setRequests[simId][key] = new SimCapiSetRequestCallback(successDelegate, errorDelegate);

            string jsonString = SimCapiJsonMaker.create_SET_DATA_REQUEST(_handshake, key, value, simId);

            if (_handshake.authToken == null)
                _pendingMessagesForHandshake.Add(jsonString);
            else
                sendMessage(jsonString);

            return true;
        }

        public bool getDataRequest(string simId, string key, GetDataRequestSuccessDelegate successDelegate, GetDataRequestErrorDelegate errorDelegate)
        {
            if (simId == null)
                throw new System.Exception("getDataRequest() called with null simId!");

            if (key == null)
                throw new System.Exception("getDataRequest() called with null key!");

            // Use local data fallback
            if (!ExternalCalls.isInIFrame() || ExternalCalls.isInAuthor())
            {
                _simCapiLocalData.getData(simId, key, successDelegate, errorDelegate);
                return true;
            }

            // Make sure dictionary exists for simId
            if (_getRequests.ContainsKey(simId) == false)
                _getRequests[simId] = new Dictionary<string, SimCapiGetRequestCallback>();


            // getRequest in progress, add to queue
            if (_getRequests[simId].ContainsKey(key) == true)
            {
                _getRequests[simId][key].queuedGetRequest = new SimCapiQueuedGetRequest(successDelegate, errorDelegate);
                return false;
            }

            _getRequests[simId][key] = new SimCapiGetRequestCallback(successDelegate, errorDelegate);

            string jsonString = SimCapiJsonMaker.create_GET_DATA_REQUEST(_handshake, key, simId);

            if (_handshake.authToken == null)
                _pendingMessagesForHandshake.Add(jsonString);
            else
                sendMessage(jsonString);

            return true;
        }


        public void requestParentContainerResize(int width, int height, ResizeSuccessDelegate resizeSuccessDelegate)
        {
            int messgeID = ++_resizeMessageId;
            string resizeMessage = SimCapiJsonMaker.create_RESIZE_PARENT_CONTAINER_REQUEST(_handshake, messgeID, width, height);

            if (resizeSuccessDelegate != null)
                _resizeCallbacks[messgeID] = resizeSuccessDelegate;

            if (_handshake.authToken == null)
                _pendingMessagesForHandshake.Add(resizeMessage);
            else
                sendMessage(resizeMessage);
        }

        public LocalDataChangedKey registerLocalDataListener(string simId, string key, LocalDataChangedDelegate localDataChangedDelegate)
        {
            string jsonString = SimCapiJsonMaker.create_REGISTER_LOCAL_DATA_CHANGE_LISTENER(_handshake, key, simId);

            if (_localDataChangedCallbacks.ContainsKey(simId) == false)
                _localDataChangedCallbacks[simId] = new Dictionary<string, LocalDataChangedKey>();

            LocalDataChangedKey newKey = new LocalDataChangedKey(localDataChangedDelegate, simId, key);
            _localDataChangedCallbacks[simId][key] = newKey;

            sendMessage(jsonString);

            return newKey;
        }

        public void unregisterLocalDataListener(LocalDataChangedKey localDataChangedKey)
        {
            string simId = localDataChangedKey.simId;
            string key = localDataChangedKey.key;

            if (_localDataChangedCallbacks[simId] == null)
                return;

            if (_localDataChangedCallbacks[simId][key] == null)
                return;

            _localDataChangedCallbacks[simId].Remove(key);

            if (_localDataChangedCallbacks[simId].Count == 0)
                _localDataChangedCallbacks.Remove(simId);

            localDataChangedKey.invalidateKey();
        }



        public void setDomainToShortform()
        {
            string currentDomain = ExternalCalls.getDomain();

            if (currentDomain.IndexOf("smartsparrow.com") == -1)
                return;

            ExternalCalls.setDomain("smartsparrow.com");
        }

        public void requestInternalViewerAccess()
        {
            string jsonString = SimCapiJsonMaker.create_ALLOW_INTERNAL_ACCESS(_handshake);
            sendMessage(jsonString);
        }


        public void setValue(string key, SimCapiData simCapiData)
        {
            if (_outGoingMap.ContainsKey(key) == false)
                return;

            _outGoingMap[key].setValueWithData(simCapiData);

            notifyValueChange();
        }


    }
}