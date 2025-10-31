using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackSim
{
    public class StrategyService
    {
        private string path = @"C:\Users\peter\OneDrive\Projects\csharp\blackjack_sim\basic_strategy.xlsx";
        public DataTable player_hard_table;
        private DataTable player_soft_table;
        private DataTable player_pair_table;
        private DataTable dealer_hard_table;
        private DataTable dealer_soft_table;

        public void BasicStrategy()
        {
            player_hard_table = ReadSheet("player_hard");
            player_soft_table = ReadSheet("player_soft");
            player_pair_table = ReadSheet("player_pair");
            dealer_hard_table = ReadSheet("dealer_hard");
            dealer_soft_table = ReadSheet("dealer_soft");
        }

        public string Decision(string ParticipantType, string HandType, int DealerHandValue, int PlayerHandValue = 0)
        {
            string dealerHandValue = Convert.ToString(DealerHandValue);
            string playerHandValue = Convert.ToString(PlayerHandValue);

            string decisionKey = $"{ParticipantType}{HandType}";

            switch (decisionKey)
            {
                case "PlayerHard":
                    return Lookup(player_hard_table, playerHandValue, dealerHandValue);
                case "PlayerSoft":
                    return Lookup(player_soft_table, playerHandValue, dealerHandValue);
                case "PlayerPair":
                    return Lookup(player_pair_table, playerHandValue, dealerHandValue);
                case "DealerHard":
                    return Lookup(dealer_hard_table, dealerHandValue);
                case "DealerSoft":
                    return Lookup(dealer_soft_table, dealerHandValue);
                default: return "Invalid";
            }
        }

        private DataTable ReadSheet(string sheetName = null)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // put in static method?

            using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
            });
            return sheetName != null ? ds.Tables[sheetName] : ds.Tables[0];
        }

        public string Lookup(DataTable table, string rowKey, string colKey = "0")
        {
            DataRow row = table.AsEnumerable()
                .First(r => r["Total"].ToString() == rowKey);
            return row[colKey].ToString();
        }
    }
}
