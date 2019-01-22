using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace SimCapi
{



    public class SimCapiValue
    {
        protected string _key;
        protected SimCapiValueType _type;
        protected bool _isReadonly;
        protected bool _isWriteonly;
        protected bool _exposed;
        protected SimCapiData _value;
        protected string[] _allowedValues;

        #region GettersAndSetters
        public string key
        {
            get
            {
                return _key;
            }
        }

        public SimCapiValueType type
        {
            get
            {
                return _type;
            }
        }

        public bool isReadonly
        {
            get
            {
                return _isReadonly;
            }
        }

        public bool isWriteonly
        {
            get
            {
                return _isWriteonly;
            }
        }

        public bool exposed
        {
            get
            {
                return _exposed;
            }

            set
            {
                _exposed = value;
            }
        }

        public SimCapiData value
        {
            get { return _value; }
        }




        #endregion



        public SimCapiValue(string key, SimCapiValueType type, bool isReadonly, bool isWriteonly, bool exposed, SimCapiData simCapiData, string[] allowedValues = null)
        {
            _key = key;
            _type = type;
            _isReadonly = isReadonly;
            _isWriteonly = isWriteonly;
            _exposed = exposed;
            _value = simCapiData;
            _allowedValues = allowedValues;
        }


        public JObject getJObjectForSerialization()
        {
            JObject jObject = new JObject();
            jObject.Add(new JProperty("key", _key));
            jObject.Add(new JProperty("type", _type));

            if (_value.GetType() == typeof(StringData))
            {
                StringData stringData = (StringData)_value;
                jObject.Add(new JProperty("value", stringData.value));
            }
            if (_value.GetType() == typeof(BoolData))
            {
                BoolData boolData = (BoolData)_value;
                jObject.Add(new JProperty("value", boolData.value));
            }
            if (_value.GetType() == typeof(NumberData))
            {
                NumberData numberData = (NumberData)_value;
                jObject.Add(new JProperty("value", numberData.value));
            }
            if(_value.GetType() == typeof(ArrayData))
            {
                ArrayData arrayData = (ArrayData)_value;
                jObject.Add(new JProperty("value", arrayData.ToString()));
            }

            jObject.Add(new JProperty("readonly", _isReadonly));
            jObject.Add(new JProperty("writeonly", _isWriteonly));
            jObject.Add(new JProperty("allowedValues", _allowedValues));
            jObject.Add(new JProperty("bindTo", null));
            return jObject;
        }

        public JObject getJObjectForSerializationForceArrays()
        {
            JObject jObject = new JObject();
            jObject.Add(new JProperty("key", _key));
            jObject.Add(new JProperty("type", _type));

            if (_value.GetType() == typeof(StringData))
            {
                StringData stringData = (StringData)_value;
                jObject.Add(new JProperty("value", stringData.value));
            }
            if (_value.GetType() == typeof(BoolData))
            {
                BoolData boolData = (BoolData)_value;
                jObject.Add(new JProperty("value", boolData.value));
            }
            if (_value.GetType() == typeof(NumberData))
            {
                NumberData numberData = (NumberData)_value;
                jObject.Add(new JProperty("value", numberData.value));
            }
            if (_value.GetType() == typeof(ArrayData))
            {
                ArrayData arrayData = (ArrayData)_value;
                jObject.Add(new JProperty("value",arrayData.value));
            }

            jObject.Add(new JProperty("readonly", _isReadonly));
            jObject.Add(new JProperty("writeonly", _isWriteonly));
            jObject.Add(new JProperty("allowedValues", _allowedValues));
            jObject.Add(new JProperty("bindTo", null));
            return jObject;
        }


        public void setValueWithData(SimCapiData data)
        {
            switch (_type)
            {
                case SimCapiValueType.NUMBER:
                    {
                        NumberData numberData = (NumberData)_value;

                        float? value = data.getNumber();

                        if (value == null)
                            SimCapiConsole.log("Cannot set " + _type.ToString() + " with " + data.GetType().ToString());
                        else
                            numberData.value = value.Value;

                        break;
                    }
                case SimCapiValueType.ARRAY:
                    {
                        ArrayData arrayData = (ArrayData)_value;

                        string[] value = data.getStringArray();

                        if (value == null)
                            SimCapiConsole.log("Cannot set " + _type.ToString() + " with " + data.GetType().ToString());
                        else
                            arrayData.value = value;

                        break;
                    }
                case SimCapiValueType.BOOLEAN:
                    {
                        BoolData boolData = (BoolData)_value;

                        bool? value = data.getBool();

                        if (value == null)
                            SimCapiConsole.log("Cannot set " + _type.ToString() + " with " + data.GetType().ToString());
                        else
                            boolData.value = value.Value;

                        break;
                    }

                case SimCapiValueType.STRING:
                case SimCapiValueType.ENUM:
                case SimCapiValueType.MATH_EXPR:
                case SimCapiValueType.ARRAY_POINT:
                    {
                        StringData stringData = (StringData)_value;
                        stringData.value = data.ToString();
                        break;
                    }
            }
        }

        public void setValue(SimCapiValue simCapiValue)
        {
            setValueWithData(simCapiValue._value);
        }

        public bool compare(SimCapiValue simCapiValue)
        {
            return _value.compare(simCapiValue._value);
        }

    }


}