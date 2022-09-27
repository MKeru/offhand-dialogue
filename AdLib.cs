public class AdLib
{
    public string adLib;
    public int emphasis;
    public AdLib(string adLib, int emphasis)
    {
        this.adLib = adLib;
        this.emphasis = emphasis;
    }

    public AdLib(string adLib)
    {
        this.adLib = adLib;
        this.emphasis = 0;
    }
}