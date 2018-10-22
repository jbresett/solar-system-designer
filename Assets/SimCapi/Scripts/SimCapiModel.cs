using System.Collections;
using System.Collections.Generic;

namespace SimCapi
{

    public class SimCapiModel
    {



    Dictionary<string, SimCapiExposableValue> _valueMap;
        Transporter _transporter;

        public SimCapiModel(Transporter transporter)
        {
            _transporter = transporter;
            _transporter.addChangeListener(handleValueChange);
            _valueMap = new Dictionary<string, SimCapiExposableValue>();

        }

        public bool exposeValue(SimCapiExposableValue exposableValue, SimCapiValue simCapiValue)
        {
            if (_valueMap.ContainsKey(simCapiValue.key) == true)
                throw new System.Exception("Value already exposed under the key/name: " + exposableValue.exposedName);

            _valueMap[simCapiValue.key] = exposableValue;

            _transporter.expose(simCapiValue);

            return true;
        }

        public void exposedValueChanged(SimCapiExposableValue exposableValue, SimCapiData simCapiData)
        {
            if (_valueMap.ContainsKey(exposableValue.exposedName) == false)
                return;

            _transporter.setValue(exposableValue.exposedName, simCapiData);
        }

        public void unexposeValue(string key)
        {
            if (_valueMap.ContainsKey(key) == true)
            {
                UnityEngine.Debug.LogError("No value exposed under this key: " + key);
                return;
            }

            _valueMap.Remove(key);

            _transporter.removeValue(key);
        }

        public void logModel()
        {
            string output = "";
            foreach(KeyValuePair<string, SimCapiExposableValue> keyPair in _valueMap)
            {
                SimCapiExposableValue value = keyPair.Value;
                output += value.ToString() + "\n";
            }

            SimCapiConsole.log(output);
        }

        void handleValueChange(List<SimCapiValue> changeList)
        {
            foreach(SimCapiValue value in changeList)
            {
                updateValueInModel(value);

            }
        }

        void  updateValueInModel(SimCapiValue simCapiValue)
        {
            if(_valueMap.ContainsKey(simCapiValue.key) == false)
            {
                SimCapiConsole.log("Key not found in model");
                return;
            }

            SimCapiExposableValue exposableValue = _valueMap[simCapiValue.key];

            if (exposableValue == null)
            {
                SimCapiConsole.log("Error no linked exposed value, This should not occur");
                return;
            }

            if (exposableValue.GetType() == typeof(SimCapiNumber))
            {
                SimCapiNumber number = (SimCapiNumber)exposableValue;

                float? value = simCapiValue.value.getNumber();

                if (value == null)
                {
                    SimCapiExposableValue.invokeInvalidValueRecivedDelegate(exposableValue);
                }
                else
                {
                    SimCapiNumber.setInternalValue(number, value.Value);
                    SimCapiNumber.triggerChangeDelegate(number, ChangedBy.AELP);
                }
                
            }
            else if (exposableValue.GetType() == typeof(SimCapiString))
            {
                SimCapiString exposableString = (SimCapiString)exposableValue;
                SimCapiString.setInternalValue(exposableString, simCapiValue.value.ToString());
                SimCapiString.triggerChangeDelegate(exposableString, ChangedBy.AELP);
            }
            else if (exposableValue.GetType() == typeof(SimCapiStringArray))
            {
                SimCapiStringArray stringArray = (SimCapiStringArray)exposableValue;

                string[] sArray = simCapiValue.value.getStringArray();

                if (sArray == null)
                {
                    SimCapiExposableValue.invokeInvalidValueRecivedDelegate(exposableValue);
                }
                else
                {
                    stringArray.setWithStringArray(sArray);
                    SimCapiStringArray.triggerChangeDelegate(stringArray, ChangedBy.AELP);
                }
                    

            }
            else if (exposableValue.GetType() == typeof(SimCapiBoolean))
            {
                SimCapiBoolean boolean = (SimCapiBoolean)exposableValue;

                bool? value = simCapiValue.value.getBool();

                if (value == null)
                {
                    SimCapiExposableValue.invokeInvalidValueRecivedDelegate(exposableValue);
                }
                else
                {
                    boolean.setValue(value.Value);
                    SimCapiBoolean.triggerChangeDelegate(boolean, ChangedBy.AELP);
                }

            }
            else if (typeof(SimCapiGenericEnum).IsAssignableFrom(exposableValue.GetType()))
            {
                SimCapiGenericEnum simCapiGenericEnum = (SimCapiGenericEnum)exposableValue;

                bool valid = SimCapiGenericEnum.setInternalValue(simCapiGenericEnum, simCapiValue.value.ToString());

                if (valid == false)
                {
                    UnityEngine.Debug.LogError("simCapiValue is not a valid Enum");
                    return;
                }
                else
                {
                    SimCapiGenericEnum.triggerChangeDelegate(simCapiGenericEnum, ChangedBy.AELP);
                }
            }
            else if (exposableValue.GetType() == typeof(SimCapiMathExpression))
            {
                SimCapiMathExpression mathExpression = (SimCapiMathExpression)exposableValue;
                SimCapiMathExpression.InternalUseOnly.setInternalValue(mathExpression, simCapiValue.value.ToString());
                SimCapiMathExpression.triggerChangeDelegate(mathExpression, ChangedBy.AELP);
            }
            else if (exposableValue.GetType() == typeof(SimCapiPointArray))
            {
                SimCapiPointArray pointArray = (SimCapiPointArray)exposableValue;

                UnityEngine.Vector2[] vector2Array = simCapiValue.value.getPointArray();

                if(vector2Array == null)
                {
                    SimCapiConsole.log("Invalid PointArrayData!");
                }
                else
                {
                    SimCapiPointArray.setInternalValue(pointArray, vector2Array);
                    SimCapiPointArray.triggerChangeDelegate(pointArray, ChangedBy.AELP);
                }
            }
            else
            {
                SimCapiConsole.log("Exposed value not set");
            }
        }

    }
}