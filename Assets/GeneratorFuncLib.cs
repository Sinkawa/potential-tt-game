using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathIntLib
{
    public static class MathInt
    {
        public static int Round(int value)
        {
            return (int) Mathf.Round(value);
        }

        public static int Round(double value)
        {
            return (int) Mathf.Round( (float) value);
        }

        public static int Round(float value)
        {
            return (int) Mathf.Round(value);
        }

        public static int Average(int Sum, int Count)
        {
            float rawAverage = Sum / Count;
            return Round(rawAverage);
        }

        public static int Average(double Sum, int Count)
        {
            double rawAverage = Sum / Count;
            return Round(rawAverage);
        }

        public static int Average(float Sum, int Count)
        {
            float rawAverage = Sum / Count;
            return Round(rawAverage);
        }
        
        public static int Average(List<int> values)
        {
            int sum = Sum(values);
            int count = values.Count;
            float rawAverage = sum / count;
            return Round(rawAverage);
        }
        
        public static int Average(List<double> values)
        {
            double sum = Sum(values);
            int count = values.Count;
            double rawAverage = sum / count;
            return Round(rawAverage);
        }
        
        public static int Average(List<float> values)
        {
            float sum = Sum(values);
            int count = values.Count;
            float rawAverage = sum / count;
            return Round(rawAverage);
        }
        
        public static int Sum(List<int> values)
        {
            int result = 0;
            foreach (int value in values)
            {
                result += value;
            }
            return result;
        }

        public static double Sum(List<double> values)
        {
            double result = 0;
            foreach (double value in values)
            {
                result += value;
            }
            return result;
        }

        public static float Sum(List<float> values)
        {
            float result = 0;
            foreach (float value in values)
            {
                result += value;
            }
            return result;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }

        public static int Clamp(double value, int min, int max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return Round(value);
        }

        public static int Clamp(float value, int min, int max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return Round(value);
        }
        
        public static int GCD(int m, int n)
        {
            int gcd = 0;
            for (int i = 1; i < (n * m + 1); i++)
            {
                if (m % i == 0 && n % i == 0)
                {
                    gcd = i;
                }
            }
            return gcd;
        }
        
    }

    public static class RandomInt 
    {
        public static int Range(int minInclusive, int maxInclusive, float coefficient = 1)
        {
            float rawValue = Random.Range(minInclusive, maxInclusive);
            return MathInt.Round(rawValue * coefficient);
        }
        public static int Range(float minInclusive, float maxInclusive, float coefficient = 1)
        {
            float rawValue = Random.Range(minInclusive, maxInclusive);
            return MathInt.Round(rawValue * coefficient);
        }
        public static int Range(double minInclusive, double maxInclusive, float coefficient = 1)
        {
            float rawValue = Random.Range((float) minInclusive, (float) maxInclusive);
            return MathInt.Round(rawValue * coefficient);
        }

    }
};