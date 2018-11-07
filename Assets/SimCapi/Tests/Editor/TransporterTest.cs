using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using NUnit.Framework;
using SimCapi;
using System.Reflection;

namespace SimCapiTests
{


    public class TestMessagePipe : MessagePipe
    {
        public List<string> sendMessageList;

        public int messageCount()
        {
            return sendMessageList.Count;
        }

        public void clearMessages()
        {
            sendMessageList.Clear();
        }

        public TestMessagePipe()
        {
            sendMessageList = new List<string>();
        }

        public override void receiveMessage(string message)
        {

        }

        public override void sendMessage(string message)
        {
            sendMessageList.Add(message);
        }
    }


    public class TransporterTests
    {


        public class HANDSHAKE_REQUEST
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }

            [Test]
            public void notifyOnReady_should_request_handshake()
            {
                TestMessagePipe testMessagePipe = new TestMessagePipe();

                Transporter transporter = new Transporter(testMessagePipe);

                transporter.notifyOnReady();

                Assert.AreEqual(1, testMessagePipe.sendMessageList.Count);

                JObject jObject = TestHelpers.deserializeObject(testMessagePipe.sendMessageList[0]);

                Assert.AreNotEqual(null, jObject);

                JProperty messageType = jObject.Property("type");

                Assert.AreNotEqual(null, messageType);

                Assert.AreEqual(SimCapiMessageType.HANDSHAKE_REQUEST, messageType.Value.ToObject<SimCapiMessageType>());
            }

            [Test]
            public void should_call_the_initial_setup_complete_listeners_when_running_locally()
            {
                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                bool? listenerCalled = false;

                transporter.addInitialSetupCompleteListener(
                    delegate ()
                    {
                        listenerCalled = true;
                    });


                transporter.notifyOnReady();

                Assert.AreEqual(true, listenerCalled.Value);
            }
        }

        public class CONFIG_CHANGE
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }

            [Test]
            public void should_ignore_CONFIG_CHANGE_when_authToken_does_not_match()
            {
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken");

                SimCapiHandshake handshake = new SimCapiHandshake();
                handshake.authToken = "Different_Token";
                handshake.version = "TestVersion";

                SimCapiConfig newConfig = new SimCapiConfig();
                newConfig.context = "Different_Config";

                handshake.config = newConfig;

                string configChangeMessage = SimCapiJsonMaker.DebugReciveMessages.create_CONFIG_CHANGE(handshake);

                SimCapiConfig oldConfig = transporter.getConfig();

                transporter.reciveJsonMessage(configChangeMessage);

                SimCapiConfig transporterConfig = transporter.getConfig();


                Assert.AreEqual(oldConfig.context, transporterConfig.context);
                Assert.AreNotEqual(newConfig.context, transporterConfig.context);
            }

            [Test]
            public void should_update_CONFIG_CHANGE_when_authToken_does_match()
            {
                string authToken = "SameToken";

                Transporter transporter = TestHelpers.getTransporterInConnectedState(authToken);

                SimCapiHandshake handshake = new SimCapiHandshake();
                handshake.authToken = authToken;
                handshake.version = "TestVersion";

                SimCapiConfig newConfig = new SimCapiConfig();
                newConfig.context = "Different_Config";

                handshake.config = newConfig;

                string configChangeMessage = SimCapiJsonMaker.DebugReciveMessages.create_CONFIG_CHANGE(handshake);


                transporter.reciveJsonMessage(configChangeMessage);

                SimCapiConfig transporterConfig = transporter.getConfig();

                Assert.AreEqual(newConfig.context, transporterConfig.context);
            }

            [Test]
            public void should_call_all_config_change_listeners()
            {
                string authToken = "SameToken";

                Transporter transporter = TestHelpers.getTransporterInConnectedState(authToken);

                SimCapiHandshake handshake = new SimCapiHandshake();
                handshake.authToken = authToken;
                handshake.version = "TestVersion";

                SimCapiConfig newConfig = new SimCapiConfig();
                newConfig.context = "Different_Config";
                handshake.config = newConfig;


                string configChangeMessage = SimCapiJsonMaker.DebugReciveMessages.create_CONFIG_CHANGE(handshake);


                bool? listenerA = false;
                bool? listenerB = false;

                transporter.addConfigChangeListener(
                    delegate (SimCapiConfig config)
                    {
                        listenerA = true;
                    });

                transporter.addConfigChangeListener(
                    delegate (SimCapiConfig config)
                    {
                        listenerB = true;
                    });

                transporter.reciveJsonMessage(configChangeMessage);


                Assert.AreEqual(true, listenerA);
                Assert.AreEqual(true, listenerB);
            }

            [Test]
            public void should_not_call_any_removed_config_change_listeners()
            {
                string authToken = "SameToken";

                Transporter transporter = TestHelpers.getTransporterInConnectedState(authToken);

                SimCapiHandshake handshake = new SimCapiHandshake();
                handshake.authToken = authToken;
                handshake.version = "TestVersion";

                SimCapiConfig newConfig = new SimCapiConfig();
                newConfig.context = "Different_Config";
                handshake.config = newConfig;


                string configChangeMessage = SimCapiJsonMaker.DebugReciveMessages.create_CONFIG_CHANGE(handshake);


                bool? listenerA = false;
                bool? listenerB = false;

                ConfigChangeDelegateKey keyA = transporter.addConfigChangeListener(
                    delegate (SimCapiConfig config)
                    {
                        listenerA = true;
                    });

                transporter.addConfigChangeListener(
                    delegate (SimCapiConfig config)
                    {
                        listenerB = true;
                    });

                transporter.removeConfigChangeListener(keyA);

                transporter.reciveJsonMessage(configChangeMessage);


                Assert.AreEqual(false, listenerA);
                Assert.AreEqual(true, listenerB);
            }

            [Test]
            public void should_not_call_any_config_change_listeners_when_all_removed()
            {
                string authToken = "SameToken";

                Transporter transporter = TestHelpers.getTransporterInConnectedState(authToken);

                SimCapiHandshake handshake = new SimCapiHandshake();
                handshake.authToken = authToken;
                handshake.version = "TestVersion";

                SimCapiConfig newConfig = new SimCapiConfig();
                newConfig.context = "Different_Config";
                handshake.config = newConfig;


                string configChangeMessage = SimCapiJsonMaker.DebugReciveMessages.create_CONFIG_CHANGE(handshake);


                bool? listenerA = false;
                bool? listenerB = false;

                transporter.addConfigChangeListener(
                    delegate (SimCapiConfig config)
                    {
                        listenerA = true;
                    });

                transporter.addConfigChangeListener(
                    delegate (SimCapiConfig config)
                    {
                        listenerB = true;
                    });

                transporter.removeAllConfigChangeListeners();

                transporter.reciveJsonMessage(configChangeMessage);


                Assert.AreEqual(false, listenerA);
                Assert.AreEqual(false, listenerB);
            }
        }

        public class ON_READY
        {

            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }


            [Test]
            public void should_send_ON_READY_and_VALUE_CHANGE_message_when_recived_a_valid_HANDSHAKE_RESPONSE()
            {
                // Setup the transporter to the requred state
                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                transporter.notifyOnReady();

                Assert.AreEqual(1, testMessagePipe.messageCount());

                string handshakeRequestJson = testMessagePipe.sendMessageList[0];

                Message.HandshakeRequest handshakeRequest = Message.HandshakeRequest.create(handshakeRequestJson);

                Assert.AreNotEqual(null, handshakeRequest);

                handshakeRequest.handshake.authToken = "TestAuthToken";

                // Create test config
                SimCapiConfig config = TestHelpers.createTestConfig();
                handshakeRequest.handshake.config = config;

                testMessagePipe.clearMessages();

                string handshakeResposeMessage = SimCapiJsonMaker.DebugReciveMessages.create_HANDSHAKE_RESPONSE(handshakeRequest.handshake);
                transporter.reciveJsonMessage(handshakeResposeMessage);
                transporter.update(TestHelpers.delayToGuaranteeMessageProceed());

                // Verify 2 messages were sent
                Assert.AreEqual(2, testMessagePipe.messageCount());

                string onReadyJson = testMessagePipe.sendMessageList[0];

                // deserialize first message 
                Message.OnReady onReady = Message.OnReady.create(onReadyJson);

                // Verify valid OnReady message
                Assert.AreNotEqual(null, onReady);

                string valueChangeJson = testMessagePipe.sendMessageList[1];

                // deserialize second message 
                SimCapiMessageType messageType = SimCapiMessageType.ALLOW_INTERNAL_ACCESS;
                object message = Message.deserialize(valueChangeJson, ref messageType);

                // Verify valid ValueChange message
                Assert.AreNotEqual(null, message);
                Assert.AreEqual(typeof(Message.ValueChange), message.GetType());

            }

            [Test]
            public void should_send_getDataRequest_after_successfull_handshake_response()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);


                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                int? delegateExecute = 0;


                transporter.getDataRequest("simID", "keyID",

                    // Success delegate
                    delegate(Message.GetDataResponse getDataResponse)
                    {
                        delegateExecute++;
                    },

                    // Error delegate
                    delegate (Message.GetDataResponse getDataResponse)
                    {
                        delegateExecute++;
                    });


                transporter.notifyOnReady();

                Assert.AreEqual(1, testMessagePipe.messageCount());

                Message.HandshakeRequest handshakeRequest = Message.HandshakeRequest.create(testMessagePipe.sendMessageList[0]);
                Assert.AreNotEqual(null, handshakeRequest);

                testMessagePipe.clearMessages();

                // getDataRequest should not have executed
                Assert.AreEqual(0, delegateExecute);


                SimCapiHandshake handshake = transporter.getHandshake();
                handshake.authToken = "AuthToken";

                
                string handshakeResposeMessage = SimCapiJsonMaker.DebugReciveMessages.create_HANDSHAKE_RESPONSE(handshake);
                transporter.reciveJsonMessage(handshakeResposeMessage);


                Message.GetDataRequest getDataRequest = null;

                // Find a getDataRequest Message
                foreach(string jsonMessage in testMessagePipe.sendMessageList)
                {
                    getDataRequest = Message.GetDataRequest.create(jsonMessage);

                    if (getDataRequest != null)
                        break;
                }

                Assert.AreNotEqual(null, getDataRequest);
            }
        }

        public class VALUE_CHANGE
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }

            [Test]
            public void update_model_when_VALUE_CHANGE_message_recived()
            {
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken");
                Transporter.setSingleton(transporter);

                string valueKey = "Exposed String";

                SimCapiString exposedString = new SimCapiString("Initial Value");
                exposedString.expose(valueKey, false, false);

                bool? stringChanged = false;

                exposedString.setChangeDelegate(
                    delegate (string value, ChangedBy changedBy)
                    {
                        stringChanged = true;
                    });

                // Create the VALUE_CHANGE message
                SimCapiValue simCapiValue = new SimCapiValue(valueKey, SimCapiValueType.STRING, false, false, false, new StringData("changed string"));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(valueKey, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(transporter.getHandshake(), valueDictionary);

                transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(true, stringChanged);

            }

            [Test]
            public void should_not_call_any_removed_listeners()
            {

                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken");
                Transporter.setSingleton(transporter);

                bool? callbackA = false;
                bool? callbackB = false;

                ChangeDelegateKey keyA = transporter.addChangeListener(
                    delegate (List<SimCapiValue> value_list)
                    {
                        callbackA = true;
                    });

                transporter.addChangeListener(
                    delegate (List<SimCapiValue> value_list)
                    {
                        callbackB = true;
                    });


                transporter.removeChangeListener(keyA);

                // Create the VALUE_CHANGE message
                string valueKey = "Exposed String";
                SimCapiValue simCapiValue = new SimCapiValue(valueKey, SimCapiValueType.STRING, false, false, false, new StringData("changed string"));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(valueKey, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(transporter.getHandshake(), valueDictionary);

                transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(false, callbackA);
                Assert.AreEqual(true, callbackB);
            }

            [Test]
            public void should_not_call_any_listeners_when_all_removed()
            {

                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken");
                Transporter.setSingleton(transporter);

                bool? callbackA = false;
                bool? callbackB = false;

                transporter.addChangeListener(
                    delegate (List<SimCapiValue> value_list)
                    {
                        callbackA = true;
                    });

                transporter.addChangeListener(
                    delegate (List<SimCapiValue> value_list)
                    {
                        callbackB = true;
                    });

                transporter.removeAllChangeListeners();

                // Create the VALUE_CHANGE message
                string valueKey = "Exposed String";
                SimCapiValue simCapiValue = new SimCapiValue(valueKey, SimCapiValueType.STRING, false, false, false, new StringData("changed string"));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(valueKey, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(transporter.getHandshake(), valueDictionary);

                transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(false, callbackA);
                Assert.AreEqual(false, callbackB);
            }

            [Test]
            public void should_ignore_VALUE_CHANGED_message_if_values_undefined()
            {
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken");
                Transporter.setSingleton(transporter);

                JObject message = new JObject();
                message.Add(new JProperty("type", SimCapiMessageType.VALUE_CHANGE));
                message.Add("handshake", JObject.FromObject(transporter.getHandshake()));
                message.Add(new JProperty("options", new JObject()));

                string invalidValueChangeJson = message.ToString(Formatting.None);

                Assert.DoesNotThrow(
                    delegate()
                    {
                        transporter.reciveJsonMessage(invalidValueChangeJson);
                    });
            }

            [Test]
            public void should_not_update_readonly_values()
            {

                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken");
                Transporter.setSingleton(transporter);

                bool? callbackA = false;

                transporter.addChangeListener(
                    delegate (List<SimCapiValue> value_list)
                    {
                        callbackA = true;
                    });

                // Create the VALUE_CHANGE message
                string valueKey = "Exposed String";
                SimCapiValue simCapiValue = new SimCapiValue(valueKey, SimCapiValueType.STRING, true, false, false, new StringData("changed string"));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(valueKey, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(transporter.getHandshake(), valueDictionary);

                transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(false, callbackA);
            }


            [Test]
            public void should_send_out_value_changed_event_with_unexposed_properties()
            {

                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken");
                Transporter.setSingleton(transporter);

                bool? callbackA = false;

                transporter.addChangeListener(
                    delegate (List<SimCapiValue> value_list)
                    {
                        callbackA = true;
                    });

                // Create the VALUE_CHANGE message
                string valueKey = "Exposed String";
                SimCapiValue simCapiValue = new SimCapiValue(valueKey, SimCapiValueType.STRING, false, false, false, new StringData("changed string"));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(valueKey, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(transporter.getHandshake(), valueDictionary);

                transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(true, callbackA);
            }

        }

        public class VALUE_CHANGE_REQUEST
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }

            [Test]
            public void should_send_value_change_notification()
            {
                TestMessagePipe testMessagePipe = new TestMessagePipe();

                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken", testMessagePipe);
                Transporter.setSingleton(transporter);

                testMessagePipe.clearMessages();

                string valueChangeRequestJson = SimCapiJsonMaker.DebugReciveMessages.create_VALUE_CHANGE_REQUEST(transporter.getHandshake());

                transporter.reciveJsonMessage(valueChangeRequestJson);
                transporter.update(TestHelpers.delayToGuaranteeMessageProceed());

                Assert.AreEqual(1, testMessagePipe.messageCount());

                SimCapiMessageType messageType = SimCapiMessageType.ALLOW_INTERNAL_ACCESS;
                object message = Message.deserialize(testMessagePipe.sendMessageList[0], ref messageType);

                Assert.AreEqual(typeof(Message.ValueChange), message.GetType());
            }
        }

        public class CHECK_REQUEST_AND_RESPONSE
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }


            [Test]
            public void check_completion_callback_should_be_called()
            {
                TestMessagePipe testMessagePipe = new TestMessagePipe();

                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken", testMessagePipe);
                Transporter.setSingleton(transporter);

                bool? completeCalled = false;

                transporter.triggerCheck(
                    delegate (Message.CheckCompleteResponse checkCompleteResponse)
                    {
                        completeCalled = true;
                    });


                string checkCompleteResponseJson = SimCapiJsonMaker.DebugReciveMessages.create_CHECK_COMPLETE_RESPONSE(transporter.getHandshake());

                transporter.reciveJsonMessage(checkCompleteResponseJson);

                Assert.AreEqual(true, completeCalled);

            }


            [Test]
            public void should_not_call_CheckStartResponse_callback()
            {

                TestMessagePipe testMessagePipe = new TestMessagePipe();

                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken", testMessagePipe);
                Transporter.setSingleton(transporter);

                bool? checkStartCalled = false;

                transporter.addCheckStartListener(
                    delegate (Message.CheckStartResponse checkStartResponse)
                    {
                        checkStartCalled = true;
                    }, false);



                transporter.triggerCheck();

                Assert.AreEqual(false, checkStartCalled);
            }

            [Test]
            public void should_call_CheckStartResponse_callback()
            {

                TestMessagePipe testMessagePipe = new TestMessagePipe();

                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken", testMessagePipe);
                Transporter.setSingleton(transporter);

                bool? checkStartCalled = false;

                transporter.addCheckStartListener(
                    delegate (Message.CheckStartResponse checkStartResponse)
                    {
                        checkStartCalled = true;
                    }, false);


                string checkStartResponseJson = SimCapiJsonMaker.DebugReciveMessages.create_CHECK_START_RESPONSE(transporter.getHandshake());

                transporter.reciveJsonMessage(checkStartResponseJson);

                Assert.AreEqual(true, checkStartCalled);
            }


            [Test]
            public void should_call_CheckStartResponse_callback_once()
            {

                TestMessagePipe testMessagePipe = new TestMessagePipe();

                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken", testMessagePipe);
                Transporter.setSingleton(transporter);

                int? calledCount = 0;

                transporter.addCheckStartListener(
                    delegate (Message.CheckStartResponse checkStartResponse)
                    {
                        calledCount++;
                    }, true);


                string checkStartResponseJson = SimCapiJsonMaker.DebugReciveMessages.create_CHECK_START_RESPONSE(transporter.getHandshake());

                // Called twice
                transporter.reciveJsonMessage(checkStartResponseJson);
                transporter.reciveJsonMessage(checkStartResponseJson);


                Assert.AreEqual(1, calledCount);
            }


            [Test]
            public void should_call_CheckStartResponse_callback_twice()
            {

                TestMessagePipe testMessagePipe = new TestMessagePipe();

                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken", testMessagePipe);
                Transporter.setSingleton(transporter);

                int? calledCount = 0;

                transporter.addCheckStartListener(
                    delegate (Message.CheckStartResponse checkStartResponse)
                    {
                        calledCount++;
                    }, false);


                string checkStartResponseJson = SimCapiJsonMaker.DebugReciveMessages.create_CHECK_START_RESPONSE(transporter.getHandshake());

                // Called twice
                transporter.reciveJsonMessage(checkStartResponseJson);
                transporter.reciveJsonMessage(checkStartResponseJson);


                Assert.AreEqual(2, calledCount);
            }

        }

        public class GET_DATA_REQUEST
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }


            [Test]
            public void should_place_get_data_request_in_pending_queue()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);


                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                transporter.getDataRequest("simId", "key",
                    delegate (Message.GetDataResponse getDataResponse)
                    {

                    },
                    delegate (Message.GetDataResponse getDataResponse)
                    {

                    });

                List<string> _pendingMessagesForHandshake = TestHelpers.getReferenceField<List<string>>(transporter, "_pendingMessagesForHandshake");

                Assert.AreNotEqual(null, _pendingMessagesForHandshake);
                Assert.AreEqual(1, _pendingMessagesForHandshake.Count);
            }


            [Test]
            public void should_send_a_get_data_request()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                testMessagePipe.clearMessages();

                transporter.getDataRequest("simId", "key",
                     delegate (Message.GetDataResponse getDataResponse)
                     {

                     },
                     delegate (Message.GetDataResponse getDataResponse)
                     {

                     });


                Assert.AreEqual(1, testMessagePipe.messageCount());

                Message.GetDataRequest getDataRequest = Message.GetDataRequest.create(testMessagePipe.sendMessageList[0]);

                Assert.AreNotEqual(null, getDataRequest);
            }

            [Test]
            public void should_throw_exception_with_null_simId_when_calling_getDataRequest()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                testMessagePipe.clearMessages();
                
                Assert.Catch(
                    delegate ()
                    {
                        transporter.getDataRequest(null, "key",
                             delegate (Message.GetDataResponse getDataResponse)
                             {

                             },
                             delegate (Message.GetDataResponse getDataResponse)
                             {

                             });
                    });
            }

            [Test]
            public void should_throw_exception_with_null_key_when_calling_getDataRequest()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                testMessagePipe.clearMessages();

                Assert.Catch(
                    delegate ()
                    {
                        transporter.getDataRequest("simId", null,
                             delegate (Message.GetDataResponse getDataResponse)
                             {

                             },
                             delegate (Message.GetDataResponse getDataResponse)
                             {

                             });
                    });
            }

            [Test]
            public void should_call_getData_from_SimCapiLocalData_and_callback_success_delegate()
            {
                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                testMessagePipe.clearMessages();

                bool? called = false;
                bool? exists = null;
                transporter.getDataRequest("simId", "key",
                        delegate (Message.GetDataResponse getDataResponse)
                        {
                            called = true;
                            exists = getDataResponse.exists;
                        },
                        delegate (Message.GetDataResponse getDataResponse)
                        {
                            
                        });


                Assert.AreEqual(true, called);
                Assert.AreNotEqual(null, exists);
                Assert.AreEqual(false, exists);
            }

            [Test]
            public void should_add_get_data_request_to_queue_if_request_has_already_been_send()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                testMessagePipe.clearMessages();

                string simId = "simId";
                string key = "key";


                GetDataRequestSuccessDelegate firstSuccessDelegate = delegate (Message.GetDataResponse getDataResponse)
                {

                };

                GetDataRequestErrorDelegate fisrtErrorDelegate = delegate (Message.GetDataResponse getDataResponse)
                {

                };

                GetDataRequestSuccessDelegate secondSuccessDelegate = delegate (Message.GetDataResponse getDataResponse)
                {

                };

                GetDataRequestErrorDelegate secondErrorDelegate = delegate (Message.GetDataResponse getDataResponse)
                {

                };


                // make get data request twice
                transporter.getDataRequest(simId, key, firstSuccessDelegate, fisrtErrorDelegate);
                transporter.getDataRequest(simId, key, secondSuccessDelegate, secondErrorDelegate);

                Dictionary<string, Dictionary<string, SimCapiGetRequestCallback>> _getRequests =
                    TestHelpers.getReferenceField<Dictionary<string, Dictionary<string, SimCapiGetRequestCallback>>>(transporter, "_getRequests");

                Assert.AreNotEqual(null, _getRequests);
                Assert.AreNotEqual(null, _getRequests[simId]);
                Assert.AreNotEqual(null, _getRequests[simId][key]);

                SimCapiGetRequestCallback getRequestCallback = _getRequests[simId][key];

                Assert.AreNotEqual(null, getRequestCallback.queuedGetRequest);

                Assert.AreEqual(secondSuccessDelegate, getRequestCallback.queuedGetRequest.successDelegate);
                Assert.AreEqual(secondErrorDelegate, getRequestCallback.queuedGetRequest.errorDelegate);
            }


        }

        public class GET_DATA_RESPONSE
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }

            [Test]
            public void should_recive_a_get_data_response_of_success()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);


                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                bool calledSuccess = false;

                string simId = "simId";
                string key = "key";

                transporter.getDataRequest(simId, key,
                    delegate (Message.GetDataResponse getDataResponse)
                    {
                        calledSuccess = true;
                    },
                    delegate (Message.GetDataResponse getDataResponse)
                    {

                    });

                string getDataResponseJson =
                    SimCapiJsonMaker.DebugReciveMessages.create_GET_DATA_RESPONSE(transporter.getHandshake(), "success", key, "new Value", true, simId, null);

                transporter.reciveJsonMessage(getDataResponseJson);

                Assert.AreEqual(true, calledSuccess);
            }

            [Test]
            public void should_recive_a_get_data_response_of_error()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);


                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                bool calledError = false;

                string simId = "simId";
                string key = "key";

                transporter.getDataRequest(simId, key,
                    delegate (Message.GetDataResponse getDataResponse)
                    {
                        
                    },
                    delegate (Message.GetDataResponse getDataResponse)
                    {
                        calledError = true;
                    });

                string getDataResponseJson =
                    SimCapiJsonMaker.DebugReciveMessages.create_GET_DATA_RESPONSE(transporter.getHandshake(), "error", key, null, true, simId, "There was an error!");

                transporter.reciveJsonMessage(getDataResponseJson);

                Assert.AreEqual(true, calledError);
            }

            
            [Test]
            public void should_call_the_next_queued_getDataRequest_if_it_exists()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);


                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                bool firstCall = false;


                string simId = "simId";
                string key = "key";

                transporter.getDataRequest(simId, key,
                    delegate (Message.GetDataResponse getDataResponse)
                    {
                        firstCall = true;
                    },
                    delegate (Message.GetDataResponse getDataResponse)
                    {

                    });

                transporter.getDataRequest(simId, key,
                    delegate (Message.GetDataResponse getDataResponse)
                    {

                    },
                    delegate (Message.GetDataResponse getDataResponse)
                    {

                    });


                string getDataResponseJson =
                    SimCapiJsonMaker.DebugReciveMessages.create_GET_DATA_RESPONSE(transporter.getHandshake(), "success", key, "new Value", true, simId, null);


                testMessagePipe.clearMessages();

                transporter.reciveJsonMessage(getDataResponseJson);

                Assert.AreEqual(true, firstCall);

                Assert.AreEqual(1, testMessagePipe.messageCount());

                Message.GetDataRequest getDataRequest = Message.GetDataRequest.create(testMessagePipe.sendMessageList[0]);

                Assert.AreNotEqual(null, getDataRequest);

            }

        }

        public class SET_DATA_REQUEST
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }

            [Test]
            public void should_place_set_data_request_in_pending_queue()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);


                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);
                


                transporter.setDataRequest("simId", "key", "value",
                    delegate (Message.SetDataResponse setDataResponse)
                    {

                    },
                    delegate (Message.SetDataResponse setDataResponse)
                    {

                    });

                List<string> _pendingMessagesForHandshake = TestHelpers.getReferenceField<List<string>>(transporter, "_pendingMessagesForHandshake");

                Assert.AreNotEqual(null, _pendingMessagesForHandshake);
                Assert.AreEqual(1, _pendingMessagesForHandshake.Count);
            }

            [Test]
            public void should_send_a_set_data_request()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                testMessagePipe.clearMessages();

                transporter.setDataRequest("simId", "key", "value",
                     delegate (Message.SetDataResponse setDataResponse)
                     {

                     },
                     delegate (Message.SetDataResponse setDataResponse)
                     {

                     });


                Assert.AreEqual(1, testMessagePipe.messageCount());

                Message.SetDataRequest setDataRequest = Message.SetDataRequest.create(testMessagePipe.sendMessageList[0]);

                Assert.AreNotEqual(null, setDataRequest);
            }

            [Test]
            public void should_throw_exception_with_null_simId_when_calling_setDataRequest()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                testMessagePipe.clearMessages();

                Assert.Catch(
                    delegate ()
                    {
                        transporter.setDataRequest(null, "key","value",
                             delegate (Message.SetDataResponse setDataResponse)
                             {

                             },
                             delegate (Message.SetDataResponse setDataResponse)
                             {

                             });
                    });
            }

            [Test]
            public void should_throw_exception_with_null_key_when_calling_setDataRequest()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                testMessagePipe.clearMessages();

                Assert.Catch(
                    delegate ()
                    {
                        transporter.setDataRequest("simId", null, "value",
                             delegate (Message.SetDataResponse setDataResponse)
                             {

                             },
                             delegate (Message.SetDataResponse setDataResponse)
                             {

                             });
                    });
            }

            [Test]
            public void should_throw_exception_with_null_value_when_calling_setDataRequest()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                testMessagePipe.clearMessages();

                Assert.Catch(
                    delegate ()
                    {
                        transporter.setDataRequest("simId", "key", null,
                             delegate (Message.SetDataResponse setDataResponse)
                             {

                             },
                             delegate (Message.SetDataResponse setDataResponse)
                             {

                             });
                    });
            }

            [Test]
            public void should_add_set_data_request_to_queue_if_request_has_already_been_send()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                testMessagePipe.clearMessages();

                string simId = "simId";
                string key = "key";

                SetDataRequestSuccessDelegate firstSuccessDelegate = delegate (Message.SetDataResponse setDataResponse)
                {

                };

                SetDataRequestErrorDelegate firstErrorDelegate = delegate (Message.SetDataResponse setDataResponse)
                {

                };

                SetDataRequestSuccessDelegate secondSuccessDelegate = delegate (Message.SetDataResponse setDataResponse)
                {

                };

                SetDataRequestErrorDelegate secondErrorDelegate = delegate (Message.SetDataResponse setDataResponse)
                {

                };

                // make set data request twice
                transporter.setDataRequest(simId, key, "value2", firstSuccessDelegate, firstErrorDelegate);
                transporter.setDataRequest(simId, key, "value2", secondSuccessDelegate, secondErrorDelegate);

                Dictionary<string, Dictionary<string, SimCapiSetRequestCallback>> _setRequests =
                    TestHelpers.getReferenceField<Dictionary<string, Dictionary<string, SimCapiSetRequestCallback>>>(transporter, "_setRequests");

                Assert.AreNotEqual(null, _setRequests);
                Assert.AreNotEqual(null, _setRequests[simId]);
                Assert.AreNotEqual(null, _setRequests[simId][key]);

                SimCapiSetRequestCallback setRequestCallback = _setRequests[simId][key];

                Assert.AreNotEqual(null, setRequestCallback.queuedSetRequest);

                Assert.AreEqual(secondSuccessDelegate, setRequestCallback.queuedSetRequest.successDelegate);
                Assert.AreEqual(secondErrorDelegate, setRequestCallback.queuedSetRequest.errorDelegate);
            }




        }

        public class SET_DATA_RESPONSE
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }

            [Test]
            public void should_receive_a_set_data_response_of_success()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("testToken", testMessagePipe);


                string simId = "simId";
                string key = "key";
                string value = "value";


                bool successCalled = false;
                bool errorCalled = false;

                transporter.setDataRequest(simId, key, value,
                    delegate (Message.SetDataResponse setDataResponse)
                    {
                        successCalled = true;
                    },
                    delegate (Message.SetDataResponse setDataResponse)
                    {
                        errorCalled = true;
                    });

                string setDataResponseJson = SimCapiJsonMaker.DebugReciveMessages.create_SET_DATA_RESPONSE(transporter.getHandshake(), "success", key, value, simId, null);

                transporter.reciveJsonMessage(setDataResponseJson);

                Assert.AreEqual(true, successCalled);
                Assert.AreEqual(false, errorCalled);

            }

            [Test]
            public void should_recive_a_set_data_response_of_error()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("testToken", testMessagePipe);


                string simId = "simId";
                string key = "key";
                string value = "value";


                bool successCalled = false;
                bool errorCalled = false;

                transporter.setDataRequest(simId, key, value,
                    delegate (Message.SetDataResponse setDataResponse)
                    {
                        successCalled = true;
                    },
                    delegate (Message.SetDataResponse setDataResponse)
                    {
                        errorCalled = true;
                    });

                string setDataResponseJson = SimCapiJsonMaker.DebugReciveMessages.create_SET_DATA_RESPONSE(transporter.getHandshake(), "error", key, value, simId, "Some kind of error!");

                transporter.reciveJsonMessage(setDataResponseJson);

                Assert.AreEqual(false, successCalled);
                Assert.AreEqual(true, errorCalled);
            }


            [Test]
            public void should_call_the_next_queued_setDataRequest_if_it_exists()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);


                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestToken", testMessagePipe);

                bool firstSuccessCalled = false;
                bool firstErrorCalled = false;

                string simId = "simId";
                string key = "key";
                string value = "value";

                SetDataRequestSuccessDelegate firstSuccessDelegate =
                delegate (Message.SetDataResponse setDataResponse)
                {
                    firstSuccessCalled = true;
                };

                SetDataRequestErrorDelegate firstErrorDelegate =
                delegate (Message.SetDataResponse setDataResponse)
                {
                    firstErrorCalled = true;
                };

                SetDataRequestSuccessDelegate secondSuccessDelegate =
                delegate (Message.SetDataResponse setDataResponse)
                {

                };

                SetDataRequestErrorDelegate secondErrorDelegate =
                delegate (Message.SetDataResponse setDataResponse)
                {

                };

                // Call the getDataRequest twice
                transporter.setDataRequest(simId, key, value, firstSuccessDelegate, firstErrorDelegate);
                transporter.setDataRequest(simId, key, value, secondSuccessDelegate, secondErrorDelegate);

                string setDataResponseJson =
                    SimCapiJsonMaker.DebugReciveMessages.create_SET_DATA_RESPONSE(transporter.getHandshake(), "success", key, value, simId, null);


                testMessagePipe.clearMessages();

                transporter.reciveJsonMessage(setDataResponseJson);

                Assert.AreEqual(true, firstSuccessCalled);
                Assert.AreEqual(false, firstErrorCalled);

                Assert.AreEqual(1, testMessagePipe.messageCount());

                Message.SetDataRequest setDataRequest = Message.SetDataRequest.create(testMessagePipe.sendMessageList[0]);

                Assert.AreNotEqual(null, setDataRequest);

            }

        }

        public class SET_VALUE
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }

            [Test]
            public void value_stored_in_outGoingMap_of_unexposed_value_should_override_default_when_exposed()
            {
                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = TestHelpers.getTransporterInConnectedState("testToken", testMessagePipe);
                Transporter.setSingleton(transporter);

                string exposedName = "exposedString";
                string initialValue = "initialValue";
                string unexposedValue = "unexposedValue";

                SimCapiString exposedString = new SimCapiString(initialValue);

                // Create the VALUE_CHANGE message
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                SimCapiValue simCapiValue = new SimCapiValue(exposedName, SimCapiValueType.STRING, false, false, false, new StringData(unexposedValue));
                valueDictionary.Add(exposedName, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(transporter.getHandshake(), valueDictionary);

                // Send a value change to the transporter, should be stored unexposed
                transporter.reciveJsonMessage(valueChangedJson);

                // expose the value
                exposedString.expose(exposedName, false, false);

                Assert.AreEqual(unexposedValue, exposedString.getValue());

                string secondValue = "secondValue";
                exposedString.setValue(secondValue);

                Assert.AreEqual(secondValue, exposedString.getValue());
            }
        }

        public class INITIAL_SETUP_COMPLETE
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }

            void setupTransporterToReciveInitialSetupCompleteMessage(Transporter transporter, TestMessagePipe testMessagePipe)
            {
                transporter.notifyOnReady();

                Assert.AreEqual(1, testMessagePipe.messageCount());

                string handshakeRequestJson = testMessagePipe.sendMessageList[0];

                Message.HandshakeRequest handshakeRequest = Message.HandshakeRequest.create(handshakeRequestJson);

                Assert.AreNotEqual(null, handshakeRequest);

                handshakeRequest.handshake.authToken = "testToken";

                // Create test config
                SimCapiConfig config = TestHelpers.createTestConfig();
                handshakeRequest.handshake.config = config;

                string handshakeResposeMessage = SimCapiJsonMaker.DebugReciveMessages.create_HANDSHAKE_RESPONSE(handshakeRequest.handshake);

                testMessagePipe.clearMessages();

                transporter.reciveJsonMessage(handshakeResposeMessage);
                transporter.update(TestHelpers.delayToGuaranteeMessageProceed());

                Assert.AreEqual(2, testMessagePipe.messageCount());

                string onReadyJson = testMessagePipe.sendMessageList[0];

                // deserialize first message 
                Message.OnReady onReady = Message.OnReady.create(onReadyJson);

                // Verify valid OnReady message
                Assert.AreNotEqual(null, onReady);

                string valueChangeJson = testMessagePipe.sendMessageList[1];

                // deserialize second message 
                SimCapiMessageType messageType = SimCapiMessageType.ALLOW_INTERNAL_ACCESS;
                object message = Message.deserialize(valueChangeJson, ref messageType);

                // Verify valid ValueChange message
                Assert.AreNotEqual(null, message);
                Assert.AreEqual(typeof(Message.ValueChange), message.GetType());
            }


            [Test]
            public void should_call_every_registered_handler()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                bool firstCallbackExecuted = false;
                bool secondCallbackExecuted = false;

                InitialSetupCompleteDelegate firstDelegate = 
                    delegate()
                    {
                        firstCallbackExecuted = true;
                    };

                InitialSetupCompleteDelegate secondDelegate =
                    delegate ()
                    {
                        secondCallbackExecuted = true;
                    };

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                transporter.addInitialSetupCompleteListener(firstDelegate);
                transporter.addInitialSetupCompleteListener(secondDelegate);

                setupTransporterToReciveInitialSetupCompleteMessage(transporter, testMessagePipe);

                // Send INITIAL_SETUP_COMPLETE message
                string initialSetupCompleteJson = SimCapiJsonMaker.DebugReciveMessages.create_INITIAL_SETUP_COMPLETE(transporter.getHandshake());
                transporter.reciveJsonMessage(initialSetupCompleteJson);

                Assert.AreEqual(true, firstCallbackExecuted);
                Assert.AreEqual(true, secondCallbackExecuted);

            }

            [Test]
            public void should_not_call_any_delegates_if_removed()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                bool firstCallbackExecuted = false;
                bool secondCallbackExecuted = false;

                InitialSetupCompleteDelegate firstDelegate =
                    delegate ()
                    {
                        firstCallbackExecuted = true;
                    };

                InitialSetupCompleteDelegate secondDelegate =
                    delegate ()
                    {
                        secondCallbackExecuted = true;
                    };

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                transporter.addInitialSetupCompleteListener(firstDelegate);
                transporter.addInitialSetupCompleteListener(secondDelegate);

                transporter.removeAllInitialSetupCompleteListeners();

                setupTransporterToReciveInitialSetupCompleteMessage(transporter, testMessagePipe);

                // Send INITIAL_SETUP_COMPLETE message
                string initialSetupCompleteJson = SimCapiJsonMaker.DebugReciveMessages.create_INITIAL_SETUP_COMPLETE(transporter.getHandshake());
                transporter.reciveJsonMessage(initialSetupCompleteJson);

                Assert.AreEqual(false, firstCallbackExecuted);
                Assert.AreEqual(false, secondCallbackExecuted);
            }

            [Test]
            public void should_do_nothing_if_the_auth_token_is_wrong()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                bool firstCallbackExecuted = false;

                InitialSetupCompleteDelegate firstDelegate =
                    delegate ()
                    {
                        firstCallbackExecuted = true;
                    };


                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                transporter.addInitialSetupCompleteListener(firstDelegate);

                setupTransporterToReciveInitialSetupCompleteMessage(transporter, testMessagePipe);

                SimCapiHandshake validHandshake = transporter.getHandshake();

                // Create bad handshake
                SimCapiHandshake badHandshake = TestHelpers.createTestHandshake();
                badHandshake.requestToken = validHandshake.requestToken;
                badHandshake.config = validHandshake.config;
                badHandshake.version = validHandshake.version;

                // Make the authtoken invalid
                badHandshake.authToken = "invalidAuthToken";

                // Send INITIAL_SETUP_COMPLETE message
                string initialSetupCompleteJson = SimCapiJsonMaker.DebugReciveMessages.create_INITIAL_SETUP_COMPLETE(badHandshake);
                transporter.reciveJsonMessage(initialSetupCompleteJson);

                Assert.AreEqual(false, firstCallbackExecuted);
            }


            [Test]
            public void should_not_call_listeners_if_another_message_is_recived()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                int callsExucuted = 0;

                InitialSetupCompleteDelegate firstDelegate =
                    delegate ()
                    {
                        callsExucuted++;
                    };


                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                transporter.addInitialSetupCompleteListener(firstDelegate);

                setupTransporterToReciveInitialSetupCompleteMessage(transporter, testMessagePipe);

                // Send INITIAL_SETUP_COMPLETE message
                string initialSetupCompleteJson = SimCapiJsonMaker.DebugReciveMessages.create_INITIAL_SETUP_COMPLETE(transporter.getHandshake());

                transporter.reciveJsonMessage(initialSetupCompleteJson);
                transporter.reciveJsonMessage(initialSetupCompleteJson);

                // callsExucuted should only be called once
                Assert.AreEqual(1, callsExucuted);

            }


            [Test]
            public void should_throw_after_initial_setup_has_been_completed()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);


                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);


                setupTransporterToReciveInitialSetupCompleteMessage(transporter, testMessagePipe);

                // Send INITIAL_SETUP_COMPLETE message
                string initialSetupCompleteJson = SimCapiJsonMaker.DebugReciveMessages.create_INITIAL_SETUP_COMPLETE(transporter.getHandshake());

                transporter.reciveJsonMessage(initialSetupCompleteJson);


                InitialSetupCompleteDelegate initDelegate =
                delegate ()
                {

                };


                // Should throw System.Exception if adds a listener after connected
                Assert.Catch(
                    delegate ()
                    {
                        transporter.addInitialSetupCompleteListener(initDelegate);
                    });


            }

        }

        public class API_CALL_RESPONSE
        {
            // Implementation pending
        }

        public class EXPOSING_VALUE_AGAIN
        {
            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }

            [Test]
            public void exposing_a_value_for_second_time_should_do_nothing()
            {
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken");
                Transporter.setSingleton(transporter);

                string exposedId = "Exposed String";

                string firstStringValue = "firstValue";
                string secondStringValue = "secondValue";


                SimCapiValue firstValue = new SimCapiValue(exposedId, SimCapiValueType.STRING, false, false, false, new StringData(firstStringValue));
                SimCapiValue secondValue = new SimCapiValue(exposedId, SimCapiValueType.STRING, false, false, false, new StringData(secondStringValue));

                transporter.expose(firstValue);
                transporter.expose(secondValue);

                Dictionary<string, SimCapiValue> _outgoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outgoingMap);
                Assert.AreEqual(true, _outgoingMap.ContainsKey(exposedId));

                SimCapiValue storedValue = _outgoingMap[exposedId];

                Assert.AreEqual(SimCapiValueType.STRING ,storedValue.type);

                Assert.AreEqual(typeof(StringData), storedValue.value.GetType());

                StringData stringData = (StringData)storedValue.value;

                Assert.AreEqual(firstStringValue, stringData.value);
            }

            [Test]
            public void should_have_consistent_behaviour_even_if_that_value_is_false()
            {
                Transporter transporter = TestHelpers.getTransporterInConnectedState("TestAuthToken");
                Transporter.setSingleton(transporter);

                string exposedId = "Exposed Bool";

                bool firstBoolValue = false;
                bool secondBoolValue = true;


                SimCapiValue firstValue = new SimCapiValue(exposedId, SimCapiValueType.BOOLEAN, false, false, false, new BoolData(firstBoolValue));
                SimCapiValue secondValue = new SimCapiValue(exposedId, SimCapiValueType.BOOLEAN, false, false, false, new BoolData(secondBoolValue));

                transporter.expose(firstValue);
                transporter.expose(secondValue);

                Dictionary<string, SimCapiValue> _outgoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outgoingMap);
                Assert.AreEqual(true, _outgoingMap.ContainsKey(exposedId));

                SimCapiValue storedValue = _outgoingMap[exposedId];

                Assert.AreEqual(SimCapiValueType.BOOLEAN, storedValue.type);

                Assert.AreEqual(typeof(BoolData), storedValue.value.GetType());

                BoolData boolData = (BoolData)storedValue.value;

                Assert.AreEqual(firstBoolValue, boolData.value);
            }

            [Test]
            public void should_stringify_the_array_before_sending_the_message_to_the_platform()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);
                Transporter.setSingleton(transporter);

                string exposedName = "exposedArray";

                SimCapiStringArray simCapiStringArray = new SimCapiStringArray();

                simCapiStringArray.expose(exposedName, false, false);

                simCapiStringArray.getList().Add("1");
                simCapiStringArray.getList().Add("2");
                simCapiStringArray.getList().Add("3");

                simCapiStringArray.updateValue();


                TestHelpers.setUpTransporterInConnectedState(transporter, testMessagePipe);

                Message.ValueChange valueChange = null;

                foreach(string jsonString in testMessagePipe.sendMessageList)
                {
                    valueChange = Message.ValueChange.createWithJsonString(jsonString);

                    if (valueChange != null)
                        break;
                }

                Assert.AreNotEqual(null, valueChange);

                Assert.AreEqual(true, valueChange.values.ContainsKey(exposedName));

                Assert.AreEqual(typeof(StringData), valueChange.values[exposedName].value.GetType());
            }

        }

        public class RESIZE_PARENT_CONTAINER
        {

        }

        public class REGISTERED_LOCAL_DATA_CHANGED
        {

            [SetUp]
            public void setup()
            {
                ExternalCalls.resetState();
            }


            [Test]
            public void should_call_the_callback_associated_with_the_sim_and_key_pair()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);


                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                TestHelpers.setUpTransporterInConnectedState(transporter, testMessagePipe);
                Transporter.setSingleton(transporter);


                string simId = "simId";
                string key = "key";
                string value = "newValue";

                bool dataChangedDelegateCalled = false;

                bool recivedValue = false;

                LocalDataChangedDelegate dataChangedDelegate = 
                    delegate(Message.RegisteredLocalDataChanged registeredLocalDataChanged)
                    {
                        dataChangedDelegateCalled = true;

                        // Make sure the call back recived the proper value
                        if (registeredLocalDataChanged.value == value)
                            recivedValue = true;
                    };

                transporter.registerLocalDataListener(simId, key, dataChangedDelegate);

                string registeredLocalDataChangedJson =
                    SimCapiJsonMaker.DebugReciveMessages.create_REGISTERED_LOCAL_DATA_CHANGED(transporter.getHandshake(), simId, key, value);


                transporter.reciveJsonMessage(registeredLocalDataChangedJson);

                Assert.AreEqual(true, dataChangedDelegateCalled);
                Assert.AreEqual(true, recivedValue);

            }


            [Test]
            public void should_send_a_REGISTER_LOCAL_DATA_CHANGE_LISTENER_message()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                TestHelpers.setUpTransporterInConnectedState(transporter, testMessagePipe);
                Transporter.setSingleton(transporter);


                string simId = "simId";
                string key = "key";

                LocalDataChangedDelegate dataChangedDelegate =
                    delegate (Message.RegisteredLocalDataChanged registeredLocalDataChanged)
                    {

                    };

                testMessagePipe.clearMessages();

                transporter.registerLocalDataListener(simId, key, dataChangedDelegate);

                Assert.AreEqual(1, testMessagePipe.messageCount());

                Message.RegisterLocalDataChangeListener registerLocalDataChangeListener =
                    Message.RegisterLocalDataChangeListener.create(testMessagePipe.sendMessageList[0]);


                Assert.AreNotEqual(null, registerLocalDataChangeListener);

                Assert.AreEqual(simId, registerLocalDataChangeListener.simId);
                Assert.AreEqual(key, registerLocalDataChangeListener.key);
            }


            [Test]
            public void should_store_a_callback()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                TestHelpers.setUpTransporterInConnectedState(transporter, testMessagePipe);
                Transporter.setSingleton(transporter);


                string simId = "simId";
                string key = "key";

                LocalDataChangedDelegate dataChangedDelegate =
                    delegate (Message.RegisteredLocalDataChanged registeredLocalDataChanged)
                    {

                    };


                transporter.registerLocalDataListener(simId, key, dataChangedDelegate);

                Dictionary<string, Dictionary<string, LocalDataChangedKey>> _localDataChangedCallbacks =
                    TestHelpers.getReferenceField<Dictionary<string, Dictionary<string, LocalDataChangedKey>>>(transporter, "_localDataChangedCallbacks");


                Assert.AreNotEqual(null, _localDataChangedCallbacks);
                Assert.AreNotEqual(null, _localDataChangedCallbacks[simId]);
                Assert.AreNotEqual(null, _localDataChangedCallbacks[simId][key]);

                LocalDataChangedKey localDataChangedKey = _localDataChangedCallbacks[simId][key];
                Assert.AreEqual(dataChangedDelegate, localDataChangedKey.localDataChangedDelegate);

            }

            [Test]
            public void should_remove_a_callback()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                TestMessagePipe testMessagePipe = new TestMessagePipe();
                Transporter transporter = new Transporter(testMessagePipe);

                TestHelpers.setUpTransporterInConnectedState(transporter, testMessagePipe);
                Transporter.setSingleton(transporter);


                string simId = "simId";
                string key = "key";

                LocalDataChangedDelegate dataChangedDelegate =
                    delegate (Message.RegisteredLocalDataChanged registeredLocalDataChanged)
                    {

                    };


                LocalDataChangedKey localDataChangedKey = transporter.registerLocalDataListener(simId, key, dataChangedDelegate);

                transporter.unregisterLocalDataListener(localDataChangedKey);


                Dictionary<string, Dictionary<string, LocalDataChangedKey>> _localDataChangedCallbacks =
                    TestHelpers.getReferenceField<Dictionary<string, Dictionary<string, LocalDataChangedKey>>>(transporter, "_localDataChangedCallbacks");

                Assert.AreNotEqual(null, _localDataChangedCallbacks);
                Assert.AreEqual(false, _localDataChangedCallbacks.ContainsKey(simId));

            }

        }


    }

}
