using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using SimCapi;
using System.Reflection;
using NUnit.Framework;

namespace SimCapiTests
{

    public static class TestHelpers
    {

        /*
         * // Get private methods
        BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        MethodInfo methodInfo = transporter.GetType().GetMethod("test", bindFlags);
        methodInfo.Invoke(transporter, null);
        */


        // Helper to access private fields
        public static T getReferenceField<T>(object objectInstance, string fieldName) where T : class
        {
            Type objectType = objectInstance.GetType();
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = objectType.GetField(fieldName, bindFlags);

            if (field == null)
                return null;

            object fieldObject = field.GetValue(objectInstance);

            if (fieldObject == null)
                return null;

            if (typeof(T) != fieldObject.GetType())
                return null;

            return (T)fieldObject;
        }

        // Helper to access private fields
        public static T? getStructField<T>(object objectInstance, string fieldName) where T : struct
        {

            Type objectType = objectInstance.GetType();
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = objectType.GetField(fieldName, bindFlags);

            if (field == null)
                return null;

            object fieldObject = field.GetValue(objectInstance);

            if (fieldObject == null)
                return null;

            if (typeof(T) != fieldObject.GetType())
                return null;

            return (T)fieldObject;
        }

        public static SimCapiHandshake createTestHandshake()
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

        public static SimCapiConfig createTestConfig()
        {
            // Create test config
            SimCapiConfig config = new SimCapiConfig();
            config.context = "TEST_MODE";
            config.lessonAttempt = 0;
            config.lessonId = "TEST_LESSION_ID";
            config.questionId = "TEST_QUESTION_ID";
            config.servicesBaseUrl = "TEST_URL";
            config.userData = new SimCapiUserData();
            config.userData.givenName = "Test NAME";
            config.userData.id = "TEST_ID";
            config.userData.surname = "Test Surname";

            return config;
        }

        public static JObject deserializeObject(string jsonString)
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

        
        public static float delayToGuaranteeMessageProceed()
        {
            return Transporter.valueChangeDelay + 1.0f;
        }


        public static Transporter getTransporterInConnectedState(string authToken, TestMessagePipe testMessagePipe = null)
        {
            if (testMessagePipe == null)
                testMessagePipe = new TestMessagePipe();

            Transporter transporter = new Transporter(testMessagePipe);

            transporter.notifyOnReady();

            Assert.AreEqual(1, testMessagePipe.messageCount());

            string handshakeRequestJson = testMessagePipe.sendMessageList[0];

            Message.HandshakeRequest handshakeRequest = Message.HandshakeRequest.create(handshakeRequestJson);

            Assert.AreNotEqual(null, handshakeRequest);

            handshakeRequest.handshake.authToken = authToken;

            // Create test config
            SimCapiConfig config = createTestConfig();
            handshakeRequest.handshake.config = config;

            string handshakeResposeMessage = SimCapiJsonMaker.DebugReciveMessages.create_HANDSHAKE_RESPONSE(handshakeRequest.handshake);
            transporter.reciveJsonMessage(handshakeResposeMessage);


            return transporter;
        }


        public static void setUpTransporterInConnectedState(Transporter transporter, TestMessagePipe testMessagePipe)
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


            transporter.reciveJsonMessage(handshakeResposeMessage);

            transporter.update(delayToGuaranteeMessageProceed());


            Assert.AreEqual(3, testMessagePipe.messageCount());

            string onReadyJson = testMessagePipe.sendMessageList[1];

            // deserialize first message 
            Message.OnReady onReady = Message.OnReady.create(onReadyJson);

            // Verify valid OnReady message
            Assert.AreNotEqual(null, onReady);

            string valueChangeJson = testMessagePipe.sendMessageList[2];

            // deserialize second message 
            SimCapiMessageType messageType = SimCapiMessageType.ALLOW_INTERNAL_ACCESS;
            object message = Message.deserialize(valueChangeJson, ref messageType);

            // Verify valid ValueChange message
            Assert.AreNotEqual(null, message);
            Assert.AreEqual(typeof(Message.ValueChange), message.GetType());

            // Send INITIAL_SETUP_COMPLETE message
            string initialSetupCompleteJson = SimCapiJsonMaker.DebugReciveMessages.create_INITIAL_SETUP_COMPLETE(transporter.getHandshake());

            transporter.reciveJsonMessage(initialSetupCompleteJson);
        }


    }

}