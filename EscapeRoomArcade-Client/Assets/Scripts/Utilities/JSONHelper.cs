using UnityEngine;

public static class JsonHelper
{
    public static string WrapArray(string json)
        => "{\"array\":" + json + "}";

    public static T[] FromJson<T>(string json)
    {
        var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
