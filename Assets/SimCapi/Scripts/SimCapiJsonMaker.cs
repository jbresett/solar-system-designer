using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


namespace SimCapi
{
    public static class SimCapiJsonMaker
    {

        /*
        public static string create_API_CALL_REQUEST(SimCapiHandshake handshake, string api, string method, string uid, APIMethodParameters apiMethodParameters)
        {
            StringBuilder string_builder = new StringBuilder();
            StringWriter string_writer = new StringWriter(string_builder);

            JsonWriter json_writer = new JsonTextWriter(string_writer);

            json_writer.Formatting = Formatting.Indented;

            json_writer.WriteStartObject();

            json_writer.WritePropertyName("type");
            json_writer.WriteValue(SimCapiMessageType.API_CALL_REQUEST);

            json_writer.WritePropertyName("handshake");
            json_writer.WriteStartObject();

            json_writer.WritePropertyName("requestToken");
            json_writer.WriteValue(handshake.requestToken);

            json_writer.WritePropertyName("authToken");
            json_writer.WriteValue(handshake.authToken);

            json_writer.WritePropertyName("version");
            json_writer.WriteValue(handshake.version);

            json_writer.WritePropertyName("config");
            json_writer.WriteRawValue("null");

            json_writer.WriteEndObject();


            json_writer.WriteEndObject();

            return string_builder.ToString();

        }
        */


        public static string create_HANDSHAKE_REQUEST(SimCapiHandshake handshake)
        {
            JObject message = new JObject();
            message.Add(new JProperty("type", SimCapiMessageType.HANDSHAKE_REQUEST));
            message.Add("handshake", JObject.FromObject(handshake));
            message.Add(new JProperty("values", new JObject()));
            message.Add(new JProperty("options", new JObject()));
            return message.ToString(Formatting.None);
        }

        public static string create_ON_READY(SimCapiHandshake handshake)
        {
            JObject message = new JObject();
            message.Add(new JProperty("type", SimCapiMessageType.ON_READY));
            message.Add("handshake", JObject.FromObject(handshake));
            message.Add(new JProperty("values", new JObject()));
            message.Add(new JProperty("options", new JObject()));
            return message.ToString(Formatting.None);
        }

        public static string create_CHECK_REQUEST(SimCapiHandshake handshake)
        {
            JObject message = new JObject();
            message.Add(new JProperty("type", SimCapiMessageType.CHECK_REQUEST));
            message.Add("handshake", JObject.FromObject(handshake));
            message.Add(new JProperty("values", new JObject()));
            message.Add(new JProperty("options", new JObject()));
            return message.ToString(Formatting.None);
        }

        public static string create_RESIZE_PARENT_CONTAINER_REQUEST(SimCapiHandshake handshake, int messageId, int width, int height)
        {
            JObject message = new JObject();
            message.Add(new JProperty("type", SimCapiMessageType.RESIZE_PARENT_CONTAINER_REQUEST));
            message.Add("handshake", JObject.FromObject(handshake));
            message.Add(new JProperty("values",
                new JObject(
                    new JProperty("messageId", messageId),
                    new JProperty("width", width),
                    new JProperty("height", height)
                    )
                ));

            return message.ToString(Formatting.None);
        }

        public static string create_SET_DATA_REQUEST(SimCapiHandshake handshake, string key, string value, string simId)
        {
            JObject message = new JObject();
            message.Add(new JProperty("type", SimCapiMessageType.SET_DATA_REQUEST));
            message.Add("handshake", JObject.FromObject(handshake));
            message.Add(new JProperty("values",
                new JObject(
                    new JProperty("key", key),
                    new JProperty("value", value),
                    new JProperty("simId", simId)
                    )
                ));
            message.Add(new JProperty("options", new JObject()));

            return message.ToString(Formatting.None);
        }

        public static string create_GET_DATA_REQUEST(SimCapiHandshake handshake, string key, string simId)
        {
            JObject message = new JObject();
            message.Add(new JProperty("type", SimCapiMessageType.GET_DATA_REQUEST));
            message.Add("handshake", JObject.FromObject(handshake));
            message.Add(new JProperty("values",
                new JObject(
                    new JProperty("key", key),
                    new JProperty("simId", simId)
                    )
                ));

            return message.ToString(Formatting.None);
        }


        public static string create_REGISTER_LOCAL_DATA_CHANGE_LISTENER(SimCapiHandshake handshake, string key, string simId)
        {
            JObject message = new JObject();
            message.Add(new JProperty("type", SimCapiMessageType.REGISTER_LOCAL_DATA_CHANGE_LISTENER));
            message.Add("handshake", JObject.FromObject(handshake));
            message.Add(new JProperty("values",
                new JObject(
                    new JProperty("key", key),
                    new JProperty("simId", simId)
                    )
                ));

            return message.ToString(Formatting.None);
        }


        public static string create_ALLOW_INTERNAL_ACCESS(SimCapiHandshake handshake)
        {
            JObject message = new JObject();
            message.Add(new JProperty("type", SimCapiMessageType.ALLOW_INTERNAL_ACCESS));
            message.Add("handshake", JObject.FromObject(handshake));
            return message.ToString(Formatting.None);
        }

        public static string create_VALUE_CHANGE(SimCapiHandshake handshake, Dictionary<string, SimCapiValue> valueDictionary)
        {
             JObject values = new JObject();

            foreach (KeyValuePair<string, SimCapiValue> keyPair in valueDictionary)
            {
                values.Add(new JProperty(keyPair.Key, keyPair.Value.getJObjectForSerialization()));
            }

            JObject message = new JObject();
            message.Add(new JProperty("type", SimCapiMessageType.VALUE_CHANGE));
            message.Add("handshake", JObject.FromObject(handshake));
            message.Add(new JProperty("values", values));
            message.Add(new JProperty("options", new JObject()));

            return message.ToString(Formatting.None);
        }

        public static string create_VALUE_CHANGE_force_arrays(SimCapiHandshake handshake, Dictionary<string, SimCapiValue> valueDictionary)
        {
            JObject values = new JObject();

            foreach (KeyValuePair<string, SimCapiValue> keyPair in valueDictionary)
            {
                values.Add(new JProperty(keyPair.Key, keyPair.Value.getJObjectForSerializationForceArrays()));
            }

            JObject message = new JObject();
            message.Add(new JProperty("type", SimCapiMessageType.VALUE_CHANGE));
            message.Add("handshake", JObject.FromObject(handshake));
            message.Add(new JProperty("values", values));
            message.Add(new JProperty("options", new JObject()));

            return message.ToString(Formatting.None);
        }


        public static class DebugReciveMessages
        {
            public static string create_CONFIG_CHANGE(SimCapiHandshake handshake)
            {
                JObject message = new JObject();
                message.Add(new JProperty("type", SimCapiMessageType.CONFIG_CHANGE));
                message.Add("handshake", JObject.FromObject(handshake));
                return message.ToString(Formatting.None);
            }

            public static string create_HANDSHAKE_RESPONSE(SimCapiHandshake handshake)
            {
                JObject message = new JObject();
                message.Add(new JProperty("type", SimCapiMessageType.HANDSHAKE_RESPONSE));
                message.Add("handshake", JObject.FromObject(handshake));
                return message.ToString(Formatting.None);
            }

            public static string create_VALUE_CHANGE_REQUEST(SimCapiHandshake handshake)
            {
                JObject message = new JObject();
                message.Add(new JProperty("type", SimCapiMessageType.VALUE_CHANGE_REQUEST));
                message.Add("handshake", JObject.FromObject(handshake));
                return message.ToString(Formatting.None);
            }

            public static string create_CHECK_COMPLETE_RESPONSE(SimCapiHandshake handshake)
            {
                JObject message = new JObject();
                message.Add(new JProperty("type", SimCapiMessageType.CHECK_COMPLETE_RESPONSE));
                message.Add("handshake", JObject.FromObject(handshake));
                return message.ToString(Formatting.None);
            }

            public static string create_CHECK_START_RESPONSE(SimCapiHandshake handshake)
            {
                JObject message = new JObject();
                message.Add(new JProperty("type", SimCapiMessageType.CHECK_START_RESPONSE));
                message.Add("handshake", JObject.FromObject(handshake));
                return message.ToString(Formatting.None);
            }


            public static string create_GET_DATA_RESPONSE(SimCapiHandshake handshake,
                                                                    string responseType,
                                                                    string key,
                                                                    string value,
                                                                      bool exists,
                                                                    string simId,
                                                                    string error)
            {
                JObject message = new JObject();
                message.Add(new JProperty("type", SimCapiMessageType.GET_DATA_RESPONSE));
                message.Add("handshake", JObject.FromObject(handshake));
                message.Add(new JProperty("values",
                     new JObject(
                         new JProperty("responseType", responseType),
                         new JProperty("key", key),
                         new JProperty("value", value),
                         new JProperty("simId", simId),
                         new JProperty("error", error),
                         new JProperty("exists", exists)
                         )
                     ));
                return message.ToString(Formatting.None);
            }


            public static string create_SET_DATA_RESPONSE(SimCapiHandshake handshake,
                                                                    string responseType,
                                                                    string key,
                                                                    string value,
                                                                    string simId,
                                                                    string error)
            {
                JObject message = new JObject();
                message.Add(new JProperty("type", SimCapiMessageType.SET_DATA_RESPONSE));
                message.Add("handshake", JObject.FromObject(handshake));
                message.Add(new JProperty("values",
                     new JObject(
                         new JProperty("responseType", responseType),
                         new JProperty("key", key),
                         new JProperty("value", value),
                         new JProperty("simId", simId),
                         new JProperty("error", error)
                         )
                     ));
                return message.ToString(Formatting.None);
            }

            public static string create_INITIAL_SETUP_COMPLETE(SimCapiHandshake handshake)
            {
                JObject message = new JObject();
                message.Add(new JProperty("type", SimCapiMessageType.INITIAL_SETUP_COMPLETE));
                message.Add("handshake", JObject.FromObject(handshake));
                message.Add(new JProperty("values", new JObject()));
                message.Add(new JProperty("options", new JObject()));
                return message.ToString(Formatting.None);
            }

            public static string create_REGISTERED_LOCAL_DATA_CHANGED(SimCapiHandshake handshake, string simId, string key, string value)
            {
                JObject message = new JObject();
                message.Add(new JProperty("type", SimCapiMessageType.REGISTERED_LOCAL_DATA_CHANGED));
                message.Add("handshake", JObject.FromObject(handshake));
                message.Add(new JProperty("values",
                     new JObject(
                         new JProperty("simId", simId),
                         new JProperty("key", key),
                         new JProperty("value", value)
                         )
                     ));
                return message.ToString(Formatting.None);
            }

        }

        

    }
}