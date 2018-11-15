using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace SimCapi
{


    public delegate void NumberChangedDelegate(float value, ChangedBy changedBy);
    public delegate void StringChangedDelegate(string value, ChangedBy changedBy);
    public delegate void StringArrayChangedDelegate(string[] value, ChangedBy changedBy);
    public delegate void BooleanChangedDelegate(bool value, ChangedBy changedBy);
    public delegate void EnumChangedDelegate<T>(T value, ChangedBy changedBy) where T : System.IConvertible;
    public delegate void MathExpressionChangedDelegate(string value, ChangedBy changedBy);
    public delegate void PointArrayChangedDelegate(Vector2[] value, ChangedBy changedBy);

    public delegate void InvalidValueRecivedDelegate();




    public abstract class SimCapiExposableValue
    {
        protected string _exposedName;

        protected InvalidValueRecivedDelegate _invalidValueRecivedDelegate;

        public abstract void expose(string name, bool isReadonly, bool isWriteonly);

        public static void invokeInvalidValueRecivedDelegate(SimCapiExposableValue simCapiExposableValue)
        {
            if (simCapiExposableValue._invalidValueRecivedDelegate == null)
                return;

            simCapiExposableValue._invalidValueRecivedDelegate();
        }

        public string exposedName
        {
            get { return _exposedName; }
        }


        public void unexpose()
        {
            if (_exposedName == null)
                return;

            Transporter.getInstance().getModel().unexposeValue(_exposedName);

            _exposedName = null;
        }

        public void setInvalidValueRecivedDelegate(InvalidValueRecivedDelegate invalidValueRecivedDelegate)
        {
            _invalidValueRecivedDelegate = invalidValueRecivedDelegate;
        }


    }


    public class SimCapiNumber : SimCapiExposableValue
    {
        float _value;
        NumberChangedDelegate _numberChangedDelegate;

        public static void setInternalValue(SimCapiNumber simCapiNumber, float value)
        {
            simCapiNumber._value = value;
        }

        public static void triggerChangeDelegate(SimCapiNumber simCapiNumber, ChangedBy changedBy)
        {
            if (simCapiNumber._numberChangedDelegate == null)
                return;

            simCapiNumber._numberChangedDelegate(simCapiNumber._value, changedBy);
        }

        public float getValue()
        {
            return _value;
        }

        public void setValue(float value)
        {
            if (_value == value)
                return;

            _value = value;

            if (_numberChangedDelegate != null)
                _numberChangedDelegate(_value, ChangedBy.SIM);

            Transporter.getInstance().getModel().exposedValueChanged(this, new NumberData(_value));
        }

        public SimCapiNumber(float initalValue)
        {
            _value = initalValue;
        }


        public override void expose(string name, bool isReadonly, bool isWriteonly)
        {
            if (_exposedName != null)
                throw new Exception("Value has alread been exposed as: " + _exposedName);

            if (name == null)
                throw new Exception("Exposed name cannot be null!");

            SimCapiValue simCapiValue = new SimCapiValue(name, SimCapiValueType.NUMBER, isReadonly, isWriteonly, false, new NumberData(_value));
            bool exposed = Transporter.getInstance().getModel().exposeValue(this, simCapiValue);

            if (exposed == true)
                _exposedName = name;
        }

        public override string ToString()
        {
            return "Type: " + GetType().ToString() + " Name: " + _exposedName + " Value: " + _value.ToString();
        }

        public void setChangeDelegate(NumberChangedDelegate numberChangedDelegate)
        {
            _numberChangedDelegate = numberChangedDelegate;
        }


    }


    public class SimCapiString : SimCapiExposableValue
    {
        string _value;
        StringChangedDelegate _stringChangedDelegate;

        public static void setInternalValue(SimCapiString simCapiString, string value)
        {
            simCapiString._value = value;
        }

        public static void triggerChangeDelegate(SimCapiString simCapiString, ChangedBy changedBy)
        {
            if (simCapiString._stringChangedDelegate == null)
                return;

            simCapiString._stringChangedDelegate(simCapiString._value, changedBy);
        }


        public SimCapiString(string initalValue)
        {
            _value = initalValue;
        }

        public void setValue(string value)
        {
            if (_value == value)
                return;

            _value = value;

            if (_stringChangedDelegate != null)
                _stringChangedDelegate(_value, ChangedBy.SIM);

            Transporter.getInstance().getModel().exposedValueChanged(this, new StringData(_value));
        }

        public string getValue()
        {
            return _value;
        }

        public override void expose(string name, bool isReadonly, bool isWriteonly)
        {
            if (_exposedName != null)
                throw new Exception("Value has alread been exposed as: " + _exposedName);

            if (name == null)
                throw new Exception("Exposed name cannot be null!");

            SimCapiValue simCapiValue = new SimCapiValue(name, SimCapiValueType.STRING, isReadonly, isWriteonly, false, new StringData(_value));
            bool exposed = Transporter.getInstance().getModel().exposeValue(this, simCapiValue);

            if (exposed == true)
                _exposedName = name;
        }

        public override string ToString()
        {
            return "Type: " + GetType().ToString() + " Name: " + _exposedName + " Value: " + _value;
        }

        public void setChangeDelegate(StringChangedDelegate stringChangedDelegate)
        {
            _stringChangedDelegate = stringChangedDelegate;
        }

    }

    public class SimCapiStringArray : SimCapiExposableValue
    {
        List<string> _values;
        StringArrayChangedDelegate _stringArrayChangedDelegate;


        public static void setInternalValue(SimCapiStringArray simCapiStringArray, string[] array)
        {
            simCapiStringArray._values.Clear();
            simCapiStringArray._values.AddRange(array);
        }

        public static void triggerChangeDelegate(SimCapiStringArray simCapiStringArray, ChangedBy changedBy)
        {
            if (simCapiStringArray._stringArrayChangedDelegate == null)
                return;

            simCapiStringArray._stringArrayChangedDelegate(simCapiStringArray._values.ToArray(), changedBy);
        }


        public SimCapiStringArray()
        {
            _values = new List<string>();
        }

        public List<string> getList()
        {
            return _values;
        }

        public void updateValue()
        {
            if (_stringArrayChangedDelegate != null)
                _stringArrayChangedDelegate(_values.ToArray(), ChangedBy.SIM);

            Transporter.getInstance().getModel().exposedValueChanged(this, new ArrayData(_values.ToArray()));
        }

        public void setWithStringArray(string[] array)
        {
            _values.Clear();
            _values.AddRange(array);
        }

        public override void expose(string name, bool isReadonly, bool isWriteonly)
        {
            if (_exposedName != null)
                throw new Exception("Value has alread been exposed as: " + _exposedName);

            if (name == null)
                throw new Exception("Exposed name cannot be null!");

            SimCapiValue simCapiValue = new SimCapiValue(name, SimCapiValueType.ARRAY, isReadonly, isWriteonly, false, new ArrayData(_values.ToArray()));
            bool exposed = Transporter.getInstance().getModel().exposeValue(this, simCapiValue);

            if (exposed == true)
                _exposedName = name;
        }

        public override string ToString()
        {
            return "Type: " + GetType().ToString() + " Name: " + _exposedName + " Value: " + _values.ToString();
        }

        public void setChangeDelegate(StringArrayChangedDelegate stringArrayChangedDelegate)
        {
            _stringArrayChangedDelegate = stringArrayChangedDelegate;
        }


    }

    public class SimCapiBoolean : SimCapiExposableValue
    {
        private bool _value;
        BooleanChangedDelegate _booleanChangedDelegate;


        public static void setInternalValue(SimCapiBoolean simCapiBoolean, bool value)
        {
            simCapiBoolean._value = value;
        }
        

        public static void triggerChangeDelegate(SimCapiBoolean simCapiBoolean, ChangedBy changedBy)
        {
            if (simCapiBoolean._booleanChangedDelegate == null)
                return;

            simCapiBoolean._booleanChangedDelegate(simCapiBoolean._value, changedBy);
        }

        public SimCapiBoolean(bool initalValue)
        {
            _value = initalValue;
        }

        public void setValue(bool value)
        {
            if (_value == value)
                return;

            _value = value;

            if (_booleanChangedDelegate != null)
                _booleanChangedDelegate(_value, ChangedBy.SIM);

            Transporter.getInstance().getModel().exposedValueChanged(this, new BoolData(_value));
        }

        public bool getValue()
        {
            return _value;
        }

        public override void expose(string name, bool isReadonly, bool isWriteonly)
        {
            if (_exposedName != null)
                throw new Exception("Value has alread been exposed as: " + _exposedName);

            if (name == null)
                throw new Exception("Exposed name cannot be null!");

            SimCapiValue simCapiValue = new SimCapiValue(name, SimCapiValueType.BOOLEAN, isReadonly, isWriteonly, false, new BoolData(_value));

            bool exposed = Transporter.getInstance().getModel().exposeValue(this, simCapiValue);

            if (exposed == true)
                _exposedName = name;
        }

        public override string ToString()
        {
            return "Type: " + GetType().ToString() + " Name: " + _exposedName + " Value: " + _value.ToString();
        }

        public void setChangeDelegate(BooleanChangedDelegate booleanChangedDelegate)
        {
            _booleanChangedDelegate = booleanChangedDelegate;
        }


    }

    public abstract class SimCapiGenericEnum : SimCapiExposableValue
    {

        public static bool setInternalValue(SimCapiGenericEnum simCapiGenericEnum, string enumString)
        {
            return simCapiGenericEnum.setWithString(enumString);
        }

        public static void triggerChangeDelegate(SimCapiGenericEnum simCapiGenericEnum, ChangedBy changedBy)
        {
            simCapiGenericEnum.triggerChangeDelegate(changedBy);
        }

        public override void expose(string name, bool isReadonly, bool isWriteonly)
        {

        }

        protected abstract bool setWithString(string enumString);
        protected abstract void triggerChangeDelegate(ChangedBy changedBy);
    }

    public class SimCapiEnum<T> : SimCapiGenericEnum where T : System.IConvertible
    {
        T _value;
        string[] _enumStringArray;
        T[] _enumValueArray;

        EnumChangedDelegate<T> _enumChangedDelegate;



        public SimCapiEnum(T initalValue)
        {
            // Type must be an Enum
            if (!typeof(T).IsEnum)
                throw new System.ArgumentException("SimCapiEnum template type T must be an enumerated type");

            _value = initalValue;

            // Create _allowedValues array
            _enumValueArray = (T[])System.Enum.GetValues(_value.GetType());

            _enumStringArray = new string[_enumValueArray.Length];
            for(int i = 0; i < _enumValueArray.Length; ++i)
            {
                _enumStringArray[i] = _enumValueArray[i].ToString();
            }
        }

        public void setValue(T value)
        {
            if ((int)(object)_value == (int)(object)value)
                return;

            _value = value;

            if (_enumChangedDelegate != null)
                _enumChangedDelegate(_value, ChangedBy.SIM);

            Transporter.getInstance().getModel().exposedValueChanged(this, new StringData(_value.ToString()));
        }

        protected override bool setWithString(string enumString)
        {
            if (enumString == null)
                return false;

            for (int i = 0; i < _enumStringArray.Length; ++i)
            {
                if (_enumStringArray[i] == enumString)
                {
                    _value = _enumValueArray[i];
                    return true;
                }
            }

            return false;
        }


        public T getValue()
        {
            return _value;
        }


        public override void expose(string name, bool isReadonly, bool isWriteonly)
        {
            if (_exposedName != null)
                throw new Exception("Value has alread been exposed as: " + _exposedName);

            if (name == null)
                throw new Exception("Exposed name cannot be null!");

            SimCapiValue simCapiValue = new SimCapiValue(name, SimCapiValueType.ENUM, isReadonly, isWriteonly, false, new StringData(_value.ToString()), _enumStringArray);
            bool exposed = Transporter.getInstance().getModel().exposeValue(this, simCapiValue);

            if (exposed == true)
                _exposedName = name;
        }

        public override string ToString()
        {
            return "Type: " + GetType().ToString() + " Name: " + _exposedName + " Value: " + _value.ToString();
        }

        public void setChangeDelegate(EnumChangedDelegate<T> enumChangedDelegate)
        {
            _enumChangedDelegate = enumChangedDelegate;
        }

        protected override void triggerChangeDelegate(ChangedBy changedBy)
        {
            if (_enumChangedDelegate == null)
                return;

            _enumChangedDelegate(_value, changedBy);
        }
    }

    public class SimCapiMathExpression : SimCapiExposableValue
    {
        string _value;

        MathExpressionChangedDelegate _mathExpressionChangedDelegate;

        public static class InternalUseOnly
        {
            public static void setInternalValue(SimCapiMathExpression simCapiMathExpression, string value)
            {
                simCapiMathExpression._value = value;
            }
        }

        public static void triggerChangeDelegate(SimCapiMathExpression simCapiMathExpression, ChangedBy changedBy)
        {
            if (simCapiMathExpression._mathExpressionChangedDelegate == null)
                return;

            simCapiMathExpression._mathExpressionChangedDelegate(simCapiMathExpression._value, changedBy);
        }

        public SimCapiMathExpression(string initalValue)
        {
            _value = initalValue;
        }

        public void setValue(string value)
        {
            if (_value == value)
                return;

            _value = value;

            if (_mathExpressionChangedDelegate != null)
                _mathExpressionChangedDelegate(_value, ChangedBy.SIM);

            Transporter.getInstance().getModel().exposedValueChanged(this, new StringData(_value));
        }

        public string getValue()
        {
            return _value;
        }

        public override void expose(string name, bool isReadonly, bool isWriteonly)
        {
            if (_exposedName != null)
                throw new Exception("Value has alread been exposed as: " + _exposedName);

            if (name == null)
                throw new Exception("Exposed name cannot be null!");

            SimCapiValue simCapiValue = new SimCapiValue(name, SimCapiValueType.MATH_EXPR, isReadonly, isWriteonly, false, new StringData(_value));
            bool exposed = Transporter.getInstance().getModel().exposeValue(this, simCapiValue);

            if (exposed == true)
                _exposedName = name;
        }

        public override string ToString()
        {
            return "Type: " + GetType().ToString() + " Name: " + _exposedName + " Value: " + _value;
        }

        public void setChangeDelegate(MathExpressionChangedDelegate mathExpressionChangedDelegate)
        {
            _mathExpressionChangedDelegate = mathExpressionChangedDelegate;
        }

 
    }

    public class SimCapiPointArray : SimCapiExposableValue
    {
        List<Vector2> _pointList;

        PointArrayChangedDelegate _pointArrayChangedDelegate;



        public static void setInternalValue(SimCapiPointArray simCapiPointArray, UnityEngine.Vector2[] array)
        {
            simCapiPointArray._pointList.Clear();
            simCapiPointArray._pointList.AddRange(array);
        }
        

        public static void triggerChangeDelegate(SimCapiPointArray simCapiPointArray, ChangedBy changedBy)
        {
            if (simCapiPointArray._pointArrayChangedDelegate == null)
                return;

            simCapiPointArray._pointArrayChangedDelegate(simCapiPointArray._pointList.ToArray(), changedBy);
        }

        public SimCapiPointArray()
        {
            _pointList = new List<Vector2>();
        }

        public List<Vector2> getPointList()
        {
            return _pointList;
        }

        public void updateValue()
        {
            if (_pointArrayChangedDelegate != null)
                _pointArrayChangedDelegate(_pointList.ToArray(), ChangedBy.SIM);

            Transporter.getInstance().getModel().exposedValueChanged(this, new StringData(arrayToString()));
        }

        public string arrayToString()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            stringBuilder.Append('[');


            for(int i = 0; i < _pointList.Count; ++i)
            {
                Vector2 point = _pointList[i];

                stringBuilder.Append('(');
                stringBuilder.Append(point.x.ToString());
                stringBuilder.Append(';');
                stringBuilder.Append(point.y.ToString());
                stringBuilder.Append(')');

                if (i < _pointList.Count - 1)
                    stringBuilder.Append(',');
            }

            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }


        public override void expose(string name, bool isReadonly, bool isWriteonly)
        {
            if (_exposedName != null)
                throw new Exception("Value has alread been exposed as: " + _exposedName);

            if (name == null)
                throw new Exception("Exposed name cannot be null!");

            SimCapiValue simCapiValue = new SimCapiValue(name, SimCapiValueType.ARRAY_POINT, isReadonly, isWriteonly, false, new StringData(arrayToString()));
            bool exposed = Transporter.getInstance().getModel().exposeValue(this, simCapiValue);

            if (exposed == true)
                _exposedName = name;
        }

        public override string ToString()
        {
            return "Type: " + GetType().ToString() + " Name: " + _exposedName + " Value: " + arrayToString();
        }

        public void setChangeDelegate(PointArrayChangedDelegate pointArrayChangedDelegate)
        {
            _pointArrayChangedDelegate = pointArrayChangedDelegate;
        }


    }
}


/*
public class FloatArray : SimCapiExposableValue
{
    List<float> _values;

    public FloatArray()
    {
        _values = new List<float>();
    }

    public List<float> getList()
    {
        return _values;
    }

    string arrayToString()
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

        stringBuilder.Append('[');

        for (int i = 0; i < _values.Count; ++i)
        {
            float value = _values[i];

            stringBuilder.Append(value.ToString());

            if (i < _values.Count - 1)
                stringBuilder.Append(',');
        }

        stringBuilder.Append(']');

        return stringBuilder.ToString();
    }

    public override void expose(string name, bool isReadonly, bool isWriteonly)
    {
        if (name == null)
        {
            Debug.LogError("Name cannon be NULL");
            return;
        }

        SimCapiModel simCapiModel = SimCapiModel.getInstance();
        SimCapiValue simCapiValue = new SimCapiValue(name, SimCapiValueType.ARRAY, isReadonly, isWriteonly, false, new ArrayData(arrayToString()), this);
        bool exposed = simCapiModel.exposeValue(simCapiValue);

        if (exposed == true)
            _exposedName = name;
    }
}
*/



