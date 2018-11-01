using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;
using System;
using NUnit.Framework;
using SimCapi;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SimCapiTests
{
    public class SimCapiMessageTests
    {

        SimCapiHandshake createTestHandshake()
        {
            SimCapiHandshake handshake = new SimCapi.SimCapiHandshake();
            handshake.authToken = uuid.generate();
            handshake.requestToken = uuid.generate();
            handshake.version = "<%= version %>";
            handshake.config = new SimCapiConfig();
            handshake.config.context = "AUTHOR";
            handshake.config.lessonId = "86380";
            handshake.config.questionId = "q:1519302508090:124";
            return handshake;
        }


        [SetUp]
        public void setup()
        {

        }


        [Test]
        public void return_null_object_with_incorrectly_formmated_json_string()
        {
            SimCapiMessageType messageType = SimCapiMessageType.ALLOW_INTERNAL_ACCESS;
            object messageObject = Message.deserialize("INVALID JSON", ref messageType);

            Assert.AreEqual(null, messageObject);
        }


        [Test]
        public void throw_error_when_message_type_is_not_supported()
        {
            SimCapiMessageType messageType = SimCapiMessageType.ALLOW_INTERNAL_ACCESS;

            // Create mock message with invalid message type
            int invalidMessageType = 5000;

            JObject message = new JObject();
            message.Add(new JProperty("type", invalidMessageType));
            message.Add("handshake", JObject.FromObject(createTestHandshake()));
            message.Add(new JProperty("values",
                new JObject(
                    new JProperty("key", "RANDOMDATA"),
                    new JProperty("simId", "RANDOMDATA")
                    )
                ));


            string messageWithInvalidType = message.ToString(Formatting.None);

            TestDelegate testDelegate = delegate ()
            {
                Message.deserialize(messageWithInvalidType, ref messageType);
            };
            
            Assert.Throws<System.Exception>(testDelegate);
        }


    }

}
