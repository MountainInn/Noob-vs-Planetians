public class HealthPickupPS : MasterPS
{
    static public HealthPickupPS instance => _inst;
    static HealthPickupPS _inst;
    HealthPickupPS(){ _inst = this; }
}
