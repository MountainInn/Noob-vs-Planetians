public class MoneyPS : MasterPS
{
    static public MoneyPS instance => _inst;
    static MoneyPS _inst;
    MoneyPS(){ _inst = this; }
}
