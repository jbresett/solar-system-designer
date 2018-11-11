using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimCapi
{





    public class Message
    {
        public static JObject deserializeJson(string jsonString)
        {
            JObject jObject;
            try
            {
                jObject = JsonConvert.DeserializeObject<JObject>(jsonString);
            }
            catch (System.Exception)
            {
                return null;
            }

            return jObject;
        }

        public static System.Object deserialize(string jsonString, ref SimCapiMessageType messageType)
        {

            JObject jObject;
            try
            {
                jObject = JsonConvert.DeserializeObject<JObject>(jsonString);
            }
            catch (System.Exception)
            {
                return null;
            }

            JProperty type = jObject.Property("type");

            if (type == null)
                throw new System.Exception("Invalid message structure");

            JToken typeValue = jObject["type"];

            if (typeValue.Type != JTokenType.Integer)
                throw new System.Exception("type property should be an Integer");


            messageType = typeValue.ToObject<SimCapiMessageType>();



            switch (messageType)
            {
                case SimCapiMessageType.HANDSHAKE_RESPONSE:
                    return HandshakeResponse.create(jObject);

                case SimCapiMessageType.VALUE_CHANGE:
                    return ValueChange.create(jObject);

                case SimCapiMessageType.CONFIG_CHANGE:
                    return ConfigChange.create(jObject);

                case SimCapiMessageType.VALUE_CHANGE_REQUEST:
                    return ValueChangeRequest.create(jObject);

                case SimCapiMessageType.CHECK_COMPLETE_RESPONSE:
                    return CheckCompleteResponse.create(jObject);

                case SimCapiMessageType.CHECK_START_RESPONSE:
                    return CheckStartResponse.create(jObject);

                case SimCapiMessageType.GET_DATA_RESPONSE:
                    return GetDataResponse.create(jObject);

                case SimCapiMessageType.SET_DATA_RESPONSE:
                    return SetDataResponse.create(jObject);

                case SimCapiMessageType.API_CALL_RESPONSE:
                    return null;

                case SimCapiMessageType.INITIAL_SETUP_COMPLETE:
                    return InitialSetupComplete.create(jObject);

                case SimCapiMessageType.RESIZE_PARENT_CONTAINER_RESPONSE:
                    return ResizeParentContainerResponse.create(jObject);

                case SimCapiMessageType.ALLOW_INTERNAL_ACCESS:
                    return AllowInternalAccess.create(jObject);

                case SimCapiMessageType.REGISTERED_LOCAL_DATA_CHANGED:
                    return RegisteredLocalDataChanged.create(jObject);
            }

            throw new System.Exception("Invalid message type");
        }

        public class HandshakeResponse
        {
            public SimCapiHandshake handshake;

            public static HandshakeResponse create(JObject jObject)
            {
                JProperty handshakeProperty = jObject.Property("handshake");

                // Must have handshake
                if (handshakeProperty == null)
                    return null;

                HandshakeResponse handshakeResponse = new HandshakeResponse();

                handshakeResponse.handshake = handshakeProperty.Value.ToObject<SimCapiHandshake>();

                return handshakeResponse;
            }
        }



        public class CheckStartResponse
        {
            public SimCapiHandshake handshake;

            public static CheckStartResponse create(JObject jObject)
            {
                JProperty handshakeProperty = jObject.Property("handshake");

                // Must have handshake
                if (handshakeProperty == null)
                    return null;

                CheckStartResponse checkStartResponse = new CheckStartResponse();

                checkStartResponse.handshake = handshakeProperty.Value.ToObject<SimCapiHandshake>();

                return checkStartResponse;
            }

        }

        public class CheckCompleteResponse
        {
            public SimCapiHandshake handshake;

            public static CheckCompleteResponse create(JObject jObject)
            {
                JProperty handshakeProperty = jObject.Property("handshake");

                // Must have handshake
                if (handshakeProperty == null)
                    return null;

                CheckCompleteResponse checkCompleteResponse = new CheckCompleteResponse();

                checkCompleteResponse.handshake = handshakeProperty.Value.ToObject<SimCapiHandshake>();

                return checkCompleteResponse;
            }

        }

        public class ResizeParentContainerResponse
        {
            public SimCapiHandshake handshake;
            public int messageId;
            public string responseType;

            public static ResizeParentContainerResponse create(JObject jObject)
            {
                JProperty handshakeProperty = jObject.Property("handshake");
                JProperty valuesProperty = jObject.Property("values");

                if (handshakeProperty == null)
                    return null;

                if (valuesProperty == null)
                    return null;

                JToken messageIdProperty = valuesProperty.Value["messageId"];
                JToken responseTypeProperty = valuesProperty.Value["responseType"];

                // Check all properties are present and valid
                if (messageIdProperty == null && messageIdProperty.Type == JTokenType.Integer)
                    return null;

                if (responseTypeProperty == null && responseTypeProperty.Type == JTokenType.String)
                    return null;

                ResizeParentContainerResponse resizeParentContainerResponse = new ResizeParentContainerResponse();

                resizeParentContainerResponse.handshake = handshakeProperty.Value.ToObject<SimCapiHandshake>();
                resizeParentContainerResponse.messageId = messageIdProperty.ToObject<int>();
                resizeParentContainerResponse.responseType = responseTypeProperty.ToObject<string>();

                return resizeParentContainerResponse;
            }
        }

        public class SetDataResponse
        {
            public SimCapiHandshake handshake;
            public string responseType;
            public string key;
            public string value;
            public string simId;
            public string error;

            public static SetDataResponse create(JObject jObject)
            {
                JProperty handshakeProperty = jObject.Property("handshake");
                JProperty valuesProperty = jObject.Property("values");

                if (handshakeProperty == null)
                    return null;

                if (valuesProperty == null)
                    return null;

                JToken responseTypeProperty = valuesProperty.Value["responseType"];
                JToken keyProperty = valuesProperty.Value["key"];
                JToken valueProperty = valuesProperty.Value["value"];
                JToken simIdProperty = valuesProperty.Value["simId"];
                JToken errorProperty = valuesProperty.Value["error"];

                // Check all properties are present and valid
                if (responseTypeProperty == null && responseTypeProperty.Type == JTokenType.String)
                    return null;

                if (keyProperty == null && keyProperty.Type == JTokenType.String)
                    return null;

                if (valueProperty == null && valueProperty.Type == JTokenType.String)
                    return null;

                if (simIdProperty == null && simIdProperty.Type == JTokenType.String)
                    return null;

                SetDataResponse setDataResponse = new SetDataResponse();

                setDataResponse.handshake = handshakeProperty.Value.ToObject<SimCapiHandshake>();
                setDataResponse.responseType = responseTypeProperty.ToObject<string>();
                setDataResponse.key = keyProperty.ToObject<string>();
                setDataResponse.value = valueProperty.ToObject<string>();
                setDataResponse.simId = simIdProperty.ToObject<string>();
                setDataResponse.error = "No Error Present";

                if (errorProperty != null && errorProperty.Type == JTokenType.String)
                    setDataResponse.error = errorProperty.ToObject<string>();


                return setDataResponse;
            }
        }


        public class GetDataResponse
        {
            public SimCapiHandshake handshake;
            public string responseType;
            public string key;
            public string value;
            public bool exists;
            public string simId;
            public string error;

            public static GetDataResponse create(JObject jObject)
            {
                JProperty handshakeProperty = jObject.Property("handshake");
                JProperty valuesProperty = jObject.Property("values");

                if (handshakeProperty == null)
                    return null;

                if (valuesProperty == null)
                    return null;

                JToken responseTypeProperty = valuesProperty.Value["responseType"];
                JToken keyProperty = valuesProperty.Value["key"];
                JToken valueProperty = valuesProperty.Value["value"];
                JToken simIdProperty = valuesProperty.Value["simId"];
                JToken errorProperty = valuesProperty.Value["error"];
                JToken existsProperty = valuesProperty.Value["exists"];

                // Check all properties are present and valid
                if (responseTypeProperty == null && responseTypeProperty.Type == JTokenType.String)
                    return null;

                if (keyProperty == null && keyProperty.Type == JTokenType.String)
                    return null;

                if (simIdProperty == null && simIdProperty.Type == JTokenType.String)
                    return null;

                GetDataResponse setDataResponse = new GetDataResponse();

                setDataResponse.handshake = handshakeProperty.Value.ToObject<SimCapiHandshake>();
                setDataResponse.responseType = responseTypeProperty.ToObject<string>();
                setDataResponse.key = keyProperty.ToObject<string>();
                setDataResponse.simId = simIdProperty.ToObject<string>();
                setDataResponse.error = "No Error Present";

                if (errorProperty != null && errorProperty.Type == JTokenType.String)
                    setDataResponse.error = errorProperty.ToObject<string>();

                if (existsProperty != null && existsProperty.Type == JTokenType.Boolean)
                    setDataResponse.exists = existsProperty.ToObject<bool>();

                if (valueProperty != null && valueProperty.Type == JTokenType.String)
                    setDataResponse.value = valueProperty.ToObject<string>();

                return setDataResponse;
            }

        }


        public class RegisteredLocalDataChanged
        {
            public SimCapiHandshake handshake;
            public string key;
            public string value;
            public string simId;

            public static RegisteredLocalDataChanged create(JObject jObject)
            {
                JProperty handshakeProperty = jObject.Property("handshake");
                JProperty valuesProperty = jObject.Property("values");

                if (handshakeProperty == null)
                    return null;

                if (valuesProperty == null)
                    return null;

                JToken keyProperty = valuesProperty.Value["key"];
                JToken valueProperty = valuesProperty.Value["value"];
                JToken simIdProperty = valuesProperty.Value["simId"];

                // Check all properties are present and valid
                if (keyProperty == null && keyProperty.Type == JTokenType.String)
                    return null;

                if (valueProperty == null && valueProperty.Type == JTokenType.String)
                    return null;

                if (simIdProperty == null && simIdProperty.Type == JTokenType.String)
                    return null;

                RegisteredLocalDataChanged registeredLocalDataChanged = new RegisteredLocalDataChanged();

                registeredLocalDataChanged.handshake = handshakeProperty.Value.ToObject<SimCapiHandshake>();
                registeredLocalDataChanged.key = keyProperty.ToObject<string>();
                registeredLocalDataChanged.value = valueProperty.ToObject<string>();
                registeredLocalDataChanged.simId = simIdProperty.ToObject<string>();

                return registeredLocalDataChanged;
            }
        }

        public class AllowInternalAccess
        {
            public SimCapiHandshake handshake;

            public static AllowInternalAccess create(JObject jObject)
            {
                JProperty handshakeProperty = jObject.Property("handshake");

                if (handshakeProperty == null)
                    return null;

                AllowInternalAccess allowInternalAccess = new AllowInternalAccess();
                allowInternalAccess.handshake = handshakeProperty.Value.ToObject<SimCapiHandshake>();

                return allowInternalAccess;
            }
        }

        public class InitialSetupComplete
        {
            public SimCapiHandshake handshake;

            public static InitialSetupComplete create(JObject jObject)
            {
                JProperty handshakeProperty = jObject.Property("handshake");

                if (handshakeProperty == null)
                    return null;

                InitialSetupComplete initialSetupComplete = new InitialSetupComplete();
                initialSetupComplete.handshake = handshakeProperty.Value.ToObject<SimCapiHandshake>();

                return initialSetupComplete;
            }
        }

        public class ValueChange
        {
            public SimCapiHandshake handshake;
            public Dictionary<string, SimCapiValue> values;

            public ValueChange()
            {
                values = new Dictionary<string, SimCapiValue>();
            }

            public static string[] getStringArray(JToken jToken)
            {
                if (jToken.Type != JTokenType.Array)
                    return null;

                JArray jArray = (JArray)jToken;

                List<string> stringList = new List<string>();

                foreach (JToken item in jArray)
                {
                    if (item.Type == JTokenType.String)
                        stringList.Add(item.ToObject<string>());
                    else
                        return null;
                }

                return stringList.ToArray();
            }


            public static SimCapiValue createUnitySimCapiValue(JProperty values)
            {
                JToken p_key = values.Value["key"];
                JToken p_type = values.Value["type"];
                JToken p_value = values.Value["value"];
                JToken p_is_read_only = values.Value["readonly"];
                JToken p_is_write_only = values.Value["writeonly"];
                JToken p_allowed_values = values.Value["allowedValues"];
                //JToken p_bind_to = values.Value["bindTo"];

                // Check all needed value exist
                if (p_key == null)
                    return null;

                if (p_type == null)
                    return null;

                if (p_value == null)
                    return null;

                if (p_is_read_only == null)
                    return null;

                if (p_is_write_only == null)
                    return null;

                if (p_allowed_values == null)
                    return null;


                SimCapiValueType type_value = p_type.ToObject<SimCapiValueType>();

                string key = p_key.ToObject<string>();
                bool is_read_only = p_is_read_only.ToObject<bool>();
                bool is_write_only = p_is_write_only.ToObject<bool>();


                switch (type_value)
                {
                    case SimCapiValueType.NUMBER:
                        {
                            float value = p_value.ToObject<float>();
                            return new SimCapiValue(key, SimCapiValueType.NUMBER, is_read_only, is_write_only, false, new NumberData(value));
                        }
                    case SimCapiValueType.STRING:
                        {
                            string value = p_value.ToObject<string>();
                            return new SimCapiValue(key, SimCapiValueType.STRING, is_read_only, is_write_only, false, new StringData(value));
                        }
                    case SimCapiValueType.ARRAY:
                        {
                            if (p_value.Type == JTokenType.Array)
                            {
                                string[] arrayString = getStringArray(p_value);
                                return new SimCapiValue(key, SimCapiValueType.ARRAY, is_read_only, is_write_only, false, new ArrayData(arrayString));
                            }
                            if(p_value.Type == JTokenType.String)
                            {
                                string arrayAsString = p_value.ToString();
                                return new SimCapiValue(key, SimCapiValueType.ARRAY, is_read_only, is_write_only, false, new StringData(arrayAsString));
                            }


                            throw new System.Exception("Invalid SimCapiValueType.ARRAY, must be an Array or String!");

                        }
                    case SimCapiValueType.BOOLEAN:
                        {
                            bool value = p_value.ToObject<bool>();
                            return new SimCapiValue(key, SimCapiValueType.BOOLEAN, is_read_only, is_write_only, false, new BoolData(value));
                        }
                    case SimCapiValueType.ENUM:
                        {
                            string value = p_value.ToObject<string>();

                            string[] allowedValues = getStringArray(values.Value["allowedValues"]);

                            if (allowedValues == null)
                                throw new System.Exception("Invalid SimCapiEnum allowedValues array, must be a string array!");

                            return new SimCapiValue(key, SimCapiValueType.ENUM, is_read_only, is_write_only, false, new StringData(value), allowedValues);
                        }
                    case SimCapiValueType.MATH_EXPR:
                        {
                            string value = p_value.ToObject<string>();
                            return new SimCapiValue(key, SimCapiValueType.MATH_EXPR, is_read_only, is_write_only, false, new StringData(value));
                        }
                    case SimCapiValueType.ARRAY_POINT:
                        return null;
                }



                return null;
            }

            public static ValueChange createWithJsonString(string jsonString)
            {
                JObject jObject = deserializeJson(jsonString);

                JProperty type = jObject.Property("type");
                JProperty handshake = jObject.Property("handshake");
                JProperty values = jObject.Property("values");

                if (type == null || type.Value == null || type.Value.Type != JTokenType.Integer)
                    return null;

                if (handshake == null)
                    return null;

                if (values == null)
                    return null;

                SimCapiMessageType messageType = type.ToObject<SimCapiMessageType>();

                if (messageType != SimCapiMessageType.VALUE_CHANGE)
                    return null;

                ValueChange valueChange = new ValueChange();
                valueChange.handshake = handshake.Value.ToObject<SimCapiHandshake>();

                foreach (JProperty property in values.Values())
                {
                    SimCapiValue simCapiValue = createUnitySimCapiValue(property);

                    if (simCapiValue != null)
                        valueChange.values.Add(simCapiValue.key, simCapiValue);
                }

                return valueChange;
            }

            public static ValueChange create(JObject jObject)
            {
                JProperty handshake = jObject.Property("handshake");
                JProperty values = jObject.Property("values");

                if (handshake == null)
                    return null;

                if (values == null)
                    return null;

                ValueChange valueChange = new ValueChange();
                valueChange.handshake = handshake.Value.ToObject<SimCapiHandshake>();

                foreach (JProperty property in values.Values())
                {
                    SimCapiValue simCapiValue = createUnitySimCapiValue(property);

                    if (simCapiValue != null)
                        valueChange.values.Add(simCapiValue.key, simCapiValue);
                }

                return valueChange;
            }
        }

        public class ConfigChange
        {
            public SimCapiHandshake handshake;

            public static ConfigChange create(JObject jObject)
            {
                JProperty handshakeProperty = jObject.Property("handshake");

                if (handshakeProperty == null)
                    return null;

                ConfigChange configChange = new ConfigChange();
                configChange.handshake = handshakeProperty.Value.ToObject<SimCapiHandshake>();

                return configChange;
            }
        }

        public class ValueChangeRequest
        {
            public SimCapiHandshake handshake;

            public static ValueChangeRequest create(JObject jObject)
            {
                JProperty handshakeProperty = jObject.Property("handshake");

                if (handshakeProperty == null)
                    return null;

                ValueChangeRequest valueChangeRequest = new ValueChangeRequest();
                valueChangeRequest.handshake = handshakeProperty.Value.ToObject<SimCapiHandshake>();

                return valueChangeRequest;
            }
        }

        #region AELP_MESSAGES
        public class HandshakeRequest
        {
            public SimCapiHandshake handshake;

            public static HandshakeRequest create(string jsonString)
            {
                JObject jObject = deserializeJson(jsonString);

                JProperty type = jObject.Property("type");
                JProperty handshake = jObject.Property("handshake");

                if (type == null || type.Value == null || type.Value.Type != JTokenType.Integer)
                    return null;

                // Must have handshake
                if (handshake == null)
                    return null;

                SimCapiMessageType messageType = type.ToObject<SimCapiMessageType>();

                if (messageType != SimCapiMessageType.HANDSHAKE_REQUEST)
                    return null;

                HandshakeRequest handshakeRequest = new HandshakeRequest();

                handshakeRequest.handshake = handshake.Value.ToObject<SimCapiHandshake>();

                return handshakeRequest;
            }
        }


        public class OnReady
        {
            public SimCapiHandshake handshake;

            public static OnReady create(string jsonString)
            {
                JObject jObject = deserializeJson(jsonString);

                JProperty type = jObject.Property("type");
                JProperty handshake = jObject.Property("handshake");

                if (type == null || type.Value == null || type.Value.Type != JTokenType.Integer)
                    return null;

                // Must have handshake
                if (handshake == null)
                    return null;

                SimCapiMessageType messageType = type.ToObject<SimCapiMessageType>();

                if (messageType != SimCapiMessageType.ON_READY)
                    return null;

                OnReady onReady = new OnReady();

                onReady.handshake = handshake.Value.ToObject<SimCapiHandshake>();

                return onReady;
            }
        }

        public class GetDataRequest
        {
            public SimCapiHandshake handshake;
            public string key;
            public string simId;

            public static GetDataRequest create(string jsonString)
            {
                JObject jObject = deserializeJson(jsonString);

                JProperty type = jObject.Property("type");
                JProperty handshake = jObject.Property("handshake");
                JProperty values = jObject.Property("values");

                if (type == null || type.Value == null || type.Value.Type != JTokenType.Integer)
                    return null;

                // Must have handshake
                if (handshake == null)
                    return null;

                if (values == null)
                    return null;

                SimCapiMessageType messageType = type.ToObject<SimCapiMessageType>();

                if (messageType != SimCapiMessageType.GET_DATA_REQUEST)
                    return null;

                JToken key = values.Value["key"];
                JToken simId = values.Value["simId"];

                if (key == null && key.Type != JTokenType.String)
                    return null;

                if (simId == null && simId.Type != JTokenType.String)
                    return null;


                GetDataRequest getDataRequest = new GetDataRequest();

                getDataRequest.handshake = handshake.Value.ToObject<SimCapiHandshake>();
                getDataRequest.key = key.ToObject<string>();
                getDataRequest.simId = simId.ToObject<string>();

                return getDataRequest;
            }
        }

        public class SetDataRequest
        {
            public SimCapiHandshake handshake;
            public string key;
            public string simId;
            public string value;

            public static SetDataRequest create(string jsonString)
            {
                JObject jObject = deserializeJson(jsonString);

                JProperty type = jObject.Property("type");
                JProperty handshake = jObject.Property("handshake");
                JProperty values = jObject.Property("values");

                if (type == null || type.Value == null || type.Value.Type != JTokenType.Integer)
                    return null;

                // Must have handshake
                if (handshake == null)
                    return null;

                if (values == null)
                    return null;

                SimCapiMessageType messageType = type.ToObject<SimCapiMessageType>();

                if (messageType != SimCapiMessageType.SET_DATA_REQUEST)
                    return null;

                JToken key = values.Value["key"];
                JToken simId = values.Value["simId"];
                JToken value = values.Value["value"];

                if (key == null && key.Type != JTokenType.String)
                    return null;

                if (simId == null && simId.Type != JTokenType.String)
                    return null;

                if (value == null && value.Type != JTokenType.String)
                    return null;

                SetDataRequest setDataRequest = new SetDataRequest();

                setDataRequest.handshake = handshake.Value.ToObject<SimCapiHandshake>();
                setDataRequest.key = key.ToObject<string>();
                setDataRequest.simId = simId.ToObject<string>();
                setDataRequest.value = value.ToObject<string>();

                return setDataRequest;
            }
        }


        public class RegisterLocalDataChangeListener
        {
            public SimCapiHandshake handshake;
            public string key;
            public string simId;

            public static RegisterLocalDataChangeListener create(string jsonString)
            {
                JObject jObject = deserializeJson(jsonString);

                JProperty type = jObject.Property("type");
                JProperty handshake = jObject.Property("handshake");
                JProperty values = jObject.Property("values");

                if (type == null || type.Value == null || type.Value.Type != JTokenType.Integer)
                    return null;

                // Must have handshake
                if (handshake == null)
                    return null;

                if (values == null)
                    return null;

                SimCapiMessageType messageType = type.ToObject<SimCapiMessageType>();

                if (messageType != SimCapiMessageType.REGISTER_LOCAL_DATA_CHANGE_LISTENER)
                    return null;

                JToken key = values.Value["key"];
                JToken simId = values.Value["simId"];

                if (key == null && key.Type != JTokenType.String)
                    return null;

                if (simId == null && simId.Type != JTokenType.String)
                    return null;

                RegisterLocalDataChangeListener registerLocalDataChangeListener = new RegisterLocalDataChangeListener();

                registerLocalDataChangeListener.handshake = handshake.Value.ToObject<SimCapiHandshake>();
                registerLocalDataChangeListener.key = key.ToObject<string>();
                registerLocalDataChangeListener.simId = simId.ToObject<string>();

                return registerLocalDataChangeListener;
            }
        }


        #endregion
    }


}