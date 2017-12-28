namespace Lykke.Service.Dash.Sign.Core.Domain
{
    public class TxUnspent
    {
        public string Txid { get; set; }
        public uint Vout { get; set; }
        public string ScriptPubKey { get; set; }
        public decimal Amount { get; set; }
        public ulong Satoshis { get; set; }
    }
}
