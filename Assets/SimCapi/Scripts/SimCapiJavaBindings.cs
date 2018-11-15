
using System.Runtime.InteropServices;

public class SimCapiJavaBindings
{
    [DllImport("__Internal")]
    public static extern void postMessage(string str);

    [DllImport("__Internal")]
    public static extern void bindMessageListener(string receiverName);

    [DllImport("__Internal")]
    public static extern bool isInIFrame();

    [DllImport("__Internal")]
    public static extern bool isInAuthor();

    [DllImport("__Internal")]
    public static extern bool setKeyPairSessionStorage(string simId, string key, string value);

    [DllImport("__Internal")]
    public static extern string getKeyPairSessionStorage(string simId, string key);

    [DllImport("__Internal")]
    public static extern string getDomain();

    [DllImport("__Internal")]
    public static extern void setDomain(string newDomain);

}