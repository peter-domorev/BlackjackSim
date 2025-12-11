using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJackSim
{
    public class StrategyService : IStrategyService
    {
        private string _path = @"C:\Users\peter\OneDrive\Master Folder\_Life!\_Hobbies\_Programming\projects\csharp\blackjack_sim\basic_strategy.xlsx";
        private DataTable _playerHardTable;
        private DataTable _playerSoftTable;
        private DataTable _playerPairTable;
        private DataTable _dealerHardTable;
        private DataTable _dealerSoftTable;

        public StrategyService()
        {
            _playerHardTable = ReadSheet("player_hard");
            _playerSoftTable = ReadSheet("player_soft");
            _playerPairTable = ReadSheet("player_pair");
            _dealerHardTable = ReadSheet("dealer_hard");
            _dealerSoftTable = ReadSheet("dealer_soft");
        }

        public Decision MakeDecision(ParticipantType participantType, HandType handType, int dealerHandValue, int playerHandValue = 0) // default 0 cos of excel table?
        {
            string rowKeyPlayer = Convert.ToString(playerHandValue);
            string columnKeyDealer = Convert.ToString(dealerHandValue);

            switch (participantType)
            {
                case ParticipantType.Player:
                    switch (handType)
                    {
                        case HandType.Hard:
                            return Lookup(_playerHardTable, rowKeyPlayer, columnKeyDealer);

                        case HandType.Soft:
                            return Lookup(_playerSoftTable, rowKeyPlayer, columnKeyDealer);

                        case HandType.Pair:
                            return Lookup(_playerPairTable, rowKeyPlayer, columnKeyDealer);

                        default: throw new Exception("Invalid hand type");
                    }

                case ParticipantType.Dealer:
                    switch (handType)
                    {
                        case HandType.Hard:
                            return Lookup(_dealerHardTable, columnKeyDealer);

                        case HandType.Soft:
                            return Lookup(_dealerSoftTable, columnKeyDealer);

                        default: throw new Exception("Invalid hand type");
                    }
                default: throw new Exception("Invalid participant type");
            }
        }

        private DataTable ReadSheet(string sheetName = null)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // put in static method?

            try
            {
                using var stream = File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = ExcelReaderFactory.CreateReader(stream);
                var ds = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                });
                return sheetName != null ? ds.Tables[sheetName] : ds.Tables[0];

            }
            catch
            {
                throw new Exception("Incorrect file path");
            }
        }
        private Decision Lookup(DataTable table, string rowKey, string colKey = "0")
        {
            DataRow row = table.AsEnumerable()
                .First(r => r["Total"].ToString() == rowKey);
            string LookupDecision = row[colKey].ToString();

            Decision decision;


            switch (LookupDecision)
            {
                case "H":
                    decision = Decision.Hit;
                    break;
                case "S":
                    decision = Decision.Stand;
                    break;
                case "D":
                    decision = Decision.Double;
                    break;
                case "P":
                    decision = Decision.Split;
                    break;
                case "R":
                    decision = Decision.Surrender;
                    break;
                case "R/H":
                    decision = Decision.SurrenderOrHit;
                    break;
                default: throw new Exception("Invalid value from excel sheet");

            }

            return decision;
        }
    }
}

