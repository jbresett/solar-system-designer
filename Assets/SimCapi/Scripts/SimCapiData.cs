using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimCapi
{

    public abstract class SimCapiData
    {

        public abstract SimCapiData deepCopy();

        public bool compare(SimCapiData simCapiData)
        {
            bool equal = false;

            // Edge case for Number,
            // The string compare dosent work for numbers as the strings can be different but the number is the same
            // "8" != "8.0" but they are the same float value
            // so we have to convert the string to a float then do the compare
            if(GetType() == typeof(NumberData))
            {
                NumberData numberA = (NumberData)this;
                float? numberB = simCapiData.getNumber();

                if (numberB == null)
                    return false;

                equal = numberA.value == numberB.Value;

                //SimCapiConsole.log("NUMB |" + numberA.value.ToString() + "|" + numberB.Value.ToString() + "| equal: " + equal.ToString());
                return equal;
            }

            string a = ToString();
            string b = simCapiData.ToString();
            equal = a == b;
            //SimCapiConsole.log("|" + a + "|" + b + "| equal: " + equal.ToString());

            return equal;
        }

        public virtual float? getNumber()
        {
            return null;
        }

        public virtual string[] getStringArray()
        {
            return null;
        }

        public virtual bool? getBool()
        {
            return null;
        }

        public virtual string isValidEnum()
        {
            return null;
        }

        public virtual Vector2[] getPointArray()
        {
            return null;
        }

    }

    public class NumberData : SimCapiData
    {
        public float value;

        public NumberData(float value)
        {
            this.value = value;
        }

        public override SimCapiData deepCopy()
        {
            return new NumberData(value);
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override float? getNumber()
        {
            return value;
        }
    }



    public class StringData : SimCapiData
    {
        public string value;

        public StringData(string value)
        {
            this.value = value;
        }

        public override SimCapiData deepCopy()
        {
            return new StringData(value);
        }

        public override string ToString()
        {
            return value;
        }

        public override float? getNumber()
        {
            float number;
            bool valid = float.TryParse(value, out number);

            if (valid)
                return number;

            return null;
        }

        public override string[] getStringArray()
        {
            if (value == null)
                return null;

            if (value.Length < 2)
                return null;

            if (value[0] != '[' || value[value.Length - 1] != ']')
                return null;

            string subString = value.Substring(1, value.Length - 2);

            string[] stringArray = subString.Split(',');

            // Remove the front and back quotation marks on the array
            foreach (string value in stringArray)
            {
                if (value.Length > 0 && value[0] == '"')
                    value.Remove(0, 1);

                if (value.Length > 0 && value[value.Length - 1] == '"')
                    value.Remove(value.Length - 1, 1);
            }

            return stringArray;
        }


        public override bool? getBool()
        {
            if (value == null)
                return null;

            if (value == "true")
                return true;

            if (value == "false")
                return false;

            return null;
        }

    }

    public class BoolData : SimCapiData
    {
        public bool value;

        public BoolData(bool value)
        {
            this.value = value;
        }

        public override SimCapiData deepCopy()
        {
            return new BoolData(value);
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override bool? getBool()
        {
            return value;
        }

    }

    
    public class ArrayData : SimCapiData
    {
        public string[] value;

        public ArrayData(string[] value)
        {
            this.value = value;
        }

        public override SimCapiData deepCopy()
        {
            return new ArrayData(value);
        }

        public override string ToString()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            stringBuilder.Append('[');

            for (int i = 0; i < value.Length; ++i)
            {
                string stringValue = value[i];

                stringBuilder.Append(stringValue);

                if (i < value.Length - 1)
                    stringBuilder.Append(',');
            }

            stringBuilder.Append(']');

            return stringBuilder.ToString();

        }

        public override string[] getStringArray()
        {
            return value;
        }

        Vector2? stringToVector2(string input)
        {
            if (input == null)
                return null;

            if (input.Length < 2)
                return null;

            if (input[0] != '(')
                return null;

            if (input[input.Length - 1] != ')')
                return null;

            string subString = input.Substring(1, input.Length - 2);

            string[] vectorArray = subString.Split(';');

            if (vectorArray.Length != 2)
                return null;

            Vector2 vector;

            bool valid = float.TryParse(vectorArray[0], out vector.x);

            if (!valid)
                return null;

            valid = float.TryParse(vectorArray[1], out vector.y);

            if (!valid)
                return null;

            return vector;
        }

        public override Vector2[] getPointArray()
        {
            Vector2[] pointArray = new Vector2[value.Length];

            for(int i = 0; i < value.Length; ++i)
            {
                Vector2? point = stringToVector2(value[i]);

                if (point == null)
                    return null;

                pointArray[i] = point.Value;
            }

            return pointArray;
        }

    }

}