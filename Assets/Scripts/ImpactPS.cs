public class ImpactPS : MasterPS
{
    static public ImpactPS instance => _inst ??= FindObjectOfType<ImpactPS>();
    static ImpactPS _inst;

}
