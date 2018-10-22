using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using SimCapi;

namespace SimCapiTests
{
    public class ExposedValuesTest
    {
        public class SimCapiNumberTest
        {
            TestMessagePipe _testMessagePipe;
            Transporter _transporter;

            [SetUp]
            public void setup()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                _testMessagePipe = new TestMessagePipe();
                _transporter = new Transporter(_testMessagePipe);

                Transporter.setSingleton(_transporter);
                TestHelpers.setUpTransporterInConnectedState(_transporter, _testMessagePipe);
            }

            [Test]
            public void should_expose_value()
            {
                SimCapiNumber simCapiNumber = new SimCapiNumber(2);

                string exposedName = "exposeName";
                simCapiNumber.expose(exposedName, true, true);

                Dictionary<string, SimCapiValue> _outGoingMap = 
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(true, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_error_when_try_to_expose_twice()
            {
                SimCapiNumber simCapiNumber = new SimCapiNumber(2);

                string exposedName = "exposeName";
                simCapiNumber.expose(exposedName, true, true);

                Assert.Catch(
                    delegate ()
                    {
                        simCapiNumber.expose(exposedName, true, true);
                    });
            }

            [Test]
            public void should_do_nothing_when_unexposed_if_not_exposed()
            {
                SimCapiNumber simCapiNumber = new SimCapiNumber(2);

                string exposedName = "exposeName";
                simCapiNumber.unexpose();

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(false, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_call_on_change_delegate()
            {
                SimCapiNumber simCapiNumber = new SimCapiNumber(2);

                string exposedName = "exposeName";
                simCapiNumber.expose(exposedName, true, true);

                bool changeDelegateCalled = false;
                bool correctValue = false;


                simCapiNumber.setChangeDelegate(
                    delegate(float value, ChangedBy changedBy)
                    {
                        changeDelegateCalled = true;

                        if (value == 10)
                            correctValue = true;
                    });

                // Create the VALUE_CHANGE message
                SimCapiValue simCapiValue = new SimCapiValue(exposedName, SimCapiValueType.STRING, false, false, false, new StringData("10"));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(exposedName, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(_transporter.getHandshake(), valueDictionary);

                _transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(true, changeDelegateCalled);
                Assert.AreEqual(true, correctValue);
            }
        }

        public class SimCapiStringTest
        {
            TestMessagePipe _testMessagePipe;
            Transporter _transporter;

            [SetUp]
            public void setup()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                _testMessagePipe = new TestMessagePipe();
                _transporter = new Transporter(_testMessagePipe);

                Transporter.setSingleton(_transporter);
                TestHelpers.setUpTransporterInConnectedState(_transporter, _testMessagePipe);
            }

            [Test]
            public void should_expose_value()
            {
                SimCapiString simCapiString = new SimCapiString("InitialValue");

                string exposedName = "exposeName";
                simCapiString.expose(exposedName, true, true);

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(true, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_error_when_try_to_expose_twice()
            {
                SimCapiString simCapiString = new SimCapiString("InitialValue");

                string exposedName = "exposeName";
                simCapiString.expose(exposedName, true, true);

                Assert.Catch(
                    delegate ()
                    {
                        simCapiString.expose(exposedName, true, true);
                    });
            }

            [Test]
            public void should_do_nothing_when_unexposed_if_not_exposed()
            {
                SimCapiString simCapiString = new SimCapiString("InitialValue");

                string exposedName = "exposeName";
                simCapiString.unexpose();

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(false, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_call_on_change_delegate()
            {
                SimCapiString simCapiString = new SimCapiString("InitialValue");

                string exposedName = "exposeName";
                simCapiString.expose(exposedName, true, true);

                bool changeDelegateCalled = false;
                bool correctValue = false;

                string newValue = "newValue";


                simCapiString.setChangeDelegate(
                    delegate (string value, ChangedBy changedBy)
                    {
                        changeDelegateCalled = true;

                        if (value == newValue)
                            correctValue = true;
                    });

                // Create the VALUE_CHANGE message
                SimCapiValue simCapiValue = new SimCapiValue(exposedName, SimCapiValueType.STRING, false, false, false, new StringData(newValue));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(exposedName, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(_transporter.getHandshake(), valueDictionary);

                _transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(true, changeDelegateCalled);
                Assert.AreEqual(true, correctValue);
            }


        }

        public class SimCapiStringArrayTest
        {
            TestMessagePipe _testMessagePipe;
            Transporter _transporter;

            [SetUp]
            public void setup()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                _testMessagePipe = new TestMessagePipe();
                _transporter = new Transporter(_testMessagePipe);

                Transporter.setSingleton(_transporter);
                TestHelpers.setUpTransporterInConnectedState(_transporter, _testMessagePipe);
            }

            [Test]
            public void should_expose_value()
            {
                SimCapiStringArray simCapiStringArray = new SimCapiStringArray();
                simCapiStringArray.getList().Add("One");
                simCapiStringArray.getList().Add("Two");
                simCapiStringArray.getList().Add("Three");

                string exposedName = "exposeName";
                simCapiStringArray.expose(exposedName, true, true);

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(true, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_error_when_try_to_expose_twice()
            {
                SimCapiStringArray simCapiStringArray = new SimCapiStringArray();
                simCapiStringArray.getList().Add("One");
                simCapiStringArray.getList().Add("Two");
                simCapiStringArray.getList().Add("Three");

                string exposedName = "exposeName";
                simCapiStringArray.expose(exposedName, true, true);

                Assert.Catch(
                    delegate ()
                    {
                        simCapiStringArray.expose(exposedName, true, true);
                    });
            }

            [Test]
            public void should_do_nothing_when_unexposed_if_not_exposed()
            {
                SimCapiStringArray simCapiStringArray = new SimCapiStringArray();
                simCapiStringArray.getList().Add("One");
                simCapiStringArray.getList().Add("Two");
                simCapiStringArray.getList().Add("Three");


                string exposedName = "exposeName";
                simCapiStringArray.unexpose();

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(false, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_call_on_change_delegate()
            {
                SimCapiStringArray simCapiStringArray = new SimCapiStringArray();
                simCapiStringArray.getList().Add("One");
                simCapiStringArray.getList().Add("Two");
                simCapiStringArray.getList().Add("Three");

                string exposedName = "exposeName";
                simCapiStringArray.expose(exposedName, true, true);

                bool changeDelegateCalled = false;
                bool correctValue = false;


                simCapiStringArray.setChangeDelegate(
                    delegate (string[] stringArray, ChangedBy changedBy)
                    {
                        changeDelegateCalled = true;

                        if (stringArray.Length == 3 &&
                            stringArray[0] == "a" &&
                            stringArray[1] == "b" &&
                            stringArray[2] == "c")
                        {
                            correctValue = true;
                        }

                    });

                // Create the VALUE_CHANGE message
                SimCapiValue simCapiValue = new SimCapiValue(exposedName, SimCapiValueType.STRING, false, false, false, new ArrayData(new[] { "a", "b", "c" }));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(exposedName, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(_transporter.getHandshake(), valueDictionary);

                _transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(true, changeDelegateCalled);
                Assert.AreEqual(true, correctValue);
            }
        }

        public class SimCapiBooleanTest
        {
            TestMessagePipe _testMessagePipe;
            Transporter _transporter;

            [SetUp]
            public void setup()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                _testMessagePipe = new TestMessagePipe();
                _transporter = new Transporter(_testMessagePipe);

                Transporter.setSingleton(_transporter);
                TestHelpers.setUpTransporterInConnectedState(_transporter, _testMessagePipe);
            }

            [Test]
            public void should_expose_value()
            {
                SimCapiBoolean simCapiBoolean = new SimCapiBoolean(true);

                string exposedName = "exposeName";
                simCapiBoolean.expose(exposedName, true, true);

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(true, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_error_when_try_to_expose_twice()
            {
                SimCapiBoolean simCapiBoolean = new SimCapiBoolean(true);

                string exposedName = "exposeName";
                simCapiBoolean.expose(exposedName, true, true);

                Assert.Catch(
                    delegate ()
                    {
                        simCapiBoolean.expose(exposedName, true, true);
                    });
            }

            [Test]
            public void should_do_nothing_when_unexposed_if_not_exposed()
            {
                SimCapiBoolean simCapiBoolean = new SimCapiBoolean(true);

                string exposedName = "exposeName";
                simCapiBoolean.unexpose();

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(false, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_call_on_change_delegate()
            {
                SimCapiBoolean simCapiBoolean = new SimCapiBoolean(true);

                string exposedName = "exposeName";
                simCapiBoolean.expose(exposedName, true, true);

                bool changeDelegateCalled = false;
                bool correctValue = false;


                simCapiBoolean.setChangeDelegate(
                    delegate (bool value, ChangedBy changedBy)
                    {
                        changeDelegateCalled = true;

                        if (value == false)
                            correctValue = true;
                    });

                // Create the VALUE_CHANGE message
                SimCapiValue simCapiValue = new SimCapiValue(exposedName, SimCapiValueType.STRING, false, false, false, new StringData("false"));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(exposedName, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(_transporter.getHandshake(), valueDictionary);

                _transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(true, changeDelegateCalled);
                Assert.AreEqual(true, correctValue);
            }
        }

        public enum EnumForTest
        {
            INITIAL,
            FIRST,
            ANOTHER,
            CHANGED,
            ENUMVALUE
        }

        public class SimCapiEnumTest
        {
            TestMessagePipe _testMessagePipe;
            Transporter _transporter;

            [SetUp]
            public void setup()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                _testMessagePipe = new TestMessagePipe();
                _transporter = new Transporter(_testMessagePipe);

                Transporter.setSingleton(_transporter);
                TestHelpers.setUpTransporterInConnectedState(_transporter, _testMessagePipe);
            }

            [Test]
            public void should_expose_value()
            {
                SimCapiEnum<EnumForTest> simCapiEnum = new SimCapiEnum<EnumForTest>(EnumForTest.INITIAL);

                string exposedName = "exposeName";
                simCapiEnum.expose(exposedName, true, true);

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(true, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_error_when_try_to_expose_twice()
            {
                SimCapiEnum<EnumForTest> simCapiEnum = new SimCapiEnum<EnumForTest>(EnumForTest.INITIAL);

                string exposedName = "exposeName";
                simCapiEnum.expose(exposedName, true, true);

                Assert.Catch(
                    delegate ()
                    {
                        simCapiEnum.expose(exposedName, true, true);
                    });
            }

            [Test]
            public void should_do_nothing_when_unexposed_if_not_exposed()
            {
                SimCapiEnum<EnumForTest> simCapiEnum = new SimCapiEnum<EnumForTest>(EnumForTest.INITIAL);

                string exposedName = "exposeName";
                simCapiEnum.unexpose();

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(false, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_call_on_change_delegate()
            {
                SimCapiEnum<EnumForTest> simCapiEnum = new SimCapiEnum<EnumForTest>(EnumForTest.INITIAL);

                string exposedName = "exposeName";
                simCapiEnum.expose(exposedName, true, true);

                bool changeDelegateCalled = false;
                bool correctValue = false;


                simCapiEnum.setChangeDelegate(
                    delegate (EnumForTest value, ChangedBy changedBy)
                    {
                        changeDelegateCalled = true;

                        if (value == EnumForTest.CHANGED)
                            correctValue = true;
                    });

                // Create the VALUE_CHANGE message
                SimCapiValue simCapiValue = new SimCapiValue(exposedName, SimCapiValueType.STRING, false, false, false, new StringData("CHANGED"));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(exposedName, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(_transporter.getHandshake(), valueDictionary);

                _transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(true, changeDelegateCalled);
                Assert.AreEqual(true, correctValue);
            }
        }

        public class SimCapiMathExpressionTest
        {
            TestMessagePipe _testMessagePipe;
            Transporter _transporter;

            [SetUp]
            public void setup()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                _testMessagePipe = new TestMessagePipe();
                _transporter = new Transporter(_testMessagePipe);

                Transporter.setSingleton(_transporter);
                TestHelpers.setUpTransporterInConnectedState(_transporter, _testMessagePipe);
            }

            [Test]
            public void should_expose_value()
            {
                SimCapiMathExpression simCapiMathExpression = new SimCapiMathExpression("InitialValue");

                string exposedName = "exposeName";
                simCapiMathExpression.expose(exposedName, true, true);

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(true, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_error_when_try_to_expose_twice()
            {
                SimCapiMathExpression simCapiMathExpression = new SimCapiMathExpression("InitialValue");

                string exposedName = "exposeName";
                simCapiMathExpression.expose(exposedName, true, true);

                Assert.Catch(
                    delegate ()
                    {
                        simCapiMathExpression.expose(exposedName, true, true);
                    });
            }

            [Test]
            public void should_do_nothing_when_unexposed_if_not_exposed()
            {
                SimCapiMathExpression simCapiMathExpression = new SimCapiMathExpression("InitialValue");

                string exposedName = "exposeName";
                simCapiMathExpression.unexpose();

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(false, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_call_on_change_delegate()
            {
                SimCapiMathExpression simCapiMathExpression = new SimCapiMathExpression("InitialValue");

                string exposedName = "exposeName";
                simCapiMathExpression.expose(exposedName, true, true);

                bool changeDelegateCalled = false;
                bool correctValue = false;

                string newValue = "newValue";


                simCapiMathExpression.setChangeDelegate(
                    delegate (string value, ChangedBy changedBy)
                    {
                        changeDelegateCalled = true;

                        if (value == newValue)
                            correctValue = true;
                    });

                // Create the VALUE_CHANGE message
                SimCapiValue simCapiValue = new SimCapiValue(exposedName, SimCapiValueType.STRING, false, false, false, new StringData(newValue));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(exposedName, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE(_transporter.getHandshake(), valueDictionary);

                _transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(true, changeDelegateCalled);
                Assert.AreEqual(true, correctValue);
            }


        }

        public class SimCapiPointArrayTest
        {
            TestMessagePipe _testMessagePipe;
            Transporter _transporter;

            [SetUp]
            public void setup()
            {
                // Set external state so the data is not stored localy
                ExternalCalls.setIsInIFrame(true);
                ExternalCalls.setIsInAuthor(false);

                _testMessagePipe = new TestMessagePipe();
                _transporter = new Transporter(_testMessagePipe);

                Transporter.setSingleton(_transporter);
                TestHelpers.setUpTransporterInConnectedState(_transporter, _testMessagePipe);
            }

            [Test]
            public void should_expose_value()
            {
                SimCapiPointArray simCapiPointArray = new SimCapiPointArray();
                simCapiPointArray.getPointList().Add(new Vector2(1, 2));
                simCapiPointArray.getPointList().Add(new Vector2(3, 4));
                simCapiPointArray.getPointList().Add(new Vector2(5, 6));

                string exposedName = "exposeName";
                simCapiPointArray.expose(exposedName, true, true);

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(true, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_error_when_try_to_expose_twice()
            {
                SimCapiPointArray simCapiPointArray = new SimCapiPointArray();
                simCapiPointArray.getPointList().Add(new Vector2(1, 2));
                simCapiPointArray.getPointList().Add(new Vector2(3, 4));
                simCapiPointArray.getPointList().Add(new Vector2(5, 6));

                string exposedName = "exposeName";
                simCapiPointArray.expose(exposedName, true, true);

                Assert.Catch(
                    delegate ()
                    {
                        simCapiPointArray.expose(exposedName, true, true);
                    });
            }

            [Test]
            public void should_do_nothing_when_unexposed_if_not_exposed()
            {
                SimCapiPointArray simCapiPointArray = new SimCapiPointArray();
                simCapiPointArray.getPointList().Add(new Vector2(1, 2));
                simCapiPointArray.getPointList().Add(new Vector2(3, 4));
                simCapiPointArray.getPointList().Add(new Vector2(5, 6));


                string exposedName = "exposeName";
                simCapiPointArray.unexpose();

                Dictionary<string, SimCapiValue> _outGoingMap =
                    TestHelpers.getReferenceField<Dictionary<string, SimCapiValue>>(_transporter, "_outGoingMap");

                Assert.AreNotEqual(null, _outGoingMap);
                Assert.AreEqual(false, _outGoingMap.ContainsKey(exposedName));
            }

            [Test]
            public void should_call_on_change_delegate()
            {
                SimCapiPointArray simCapiPointArray = new SimCapiPointArray();
                simCapiPointArray.getPointList().Add(new Vector2(1, 2));
                simCapiPointArray.getPointList().Add(new Vector2(3, 4));
                simCapiPointArray.getPointList().Add(new Vector2(5, 6));

                string exposedName = "exposeName";
                simCapiPointArray.expose(exposedName, true, true);

                bool changeDelegateCalled = false;
                bool correctValue = false;


                simCapiPointArray.setChangeDelegate(
                    delegate (Vector2[] value, ChangedBy changedBy)
                    {
                        changeDelegateCalled = true;

                        if (value.Length == 3 &&
                            value[0] == new Vector2(6, 5) &&
                            value[1] == new Vector2(4, 3) &&
                            value[2] == new Vector2(2, 1))
                        {
                            correctValue = true;
                        }

                    });

                // Create the VALUE_CHANGE message
                SimCapiValue simCapiValue = new SimCapiValue(exposedName, SimCapiValueType.ARRAY, false, false, false, new ArrayData(new[] { "(6;5)", "(4;3)", "(2;1)" }));
                Dictionary<string, SimCapiValue> valueDictionary = new Dictionary<string, SimCapiValue>();
                valueDictionary.Add(exposedName, simCapiValue);

                string valueChangedJson = SimCapiJsonMaker.create_VALUE_CHANGE_force_arrays(_transporter.getHandshake(), valueDictionary);

                _transporter.reciveJsonMessage(valueChangedJson);

                Assert.AreEqual(true, changeDelegateCalled);
                Assert.AreEqual(true, correctValue);
            }
        }
    }

}