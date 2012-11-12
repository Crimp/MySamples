using ConsoleHelper.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleClient {
    public class ConsoleHelper {
        string column_1_Caption = "FirstName";
        string column_2_Caption = "LastName";
        string column_3_Caption = "Email";
        int spaceCount = 12;
        public void PrintData(IQueryable<Contact> contactQuery) {
            Console.WriteLine("-------------------------------------------");
            PrintHeader();
            foreach(Contact contact in contactQuery) {
                PrintContact(contact);
            }
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
        }

        private void PrintContact(Contact contact) {
            StringBuilder builder = new StringBuilder();
            string column1Data = contact.FirstName;
            builder.Append(column1Data);
            Insert(builder, Column_2_Position, contact.LastName);
            Insert(builder, Column_3_Position, contact.Email);
            Console.WriteLine(builder.ToString());
        }
        private void PrintHeader() {
            StringBuilder builder = new StringBuilder();
            builder.Append(column_1_Caption);
            Insert(builder, Column_2_Position, column_2_Caption);
            Insert(builder, Column_3_Position, column_3_Caption);
            Console.WriteLine(builder.ToString());
            Console.WriteLine();
        }
        private void Insert(StringBuilder builder, int position, string value) {
            if(builder.Length < position) {
                builder.Append(' ',position - builder.Length);
            }
            builder.Append(value);
        }
        private int Column_2_Position {
            get {
                return spaceCount + column_1_Caption.Length;
            }
        }
        private int Column_3_Position {
            get {
                return Column_2_Position + column_2_Caption.Length + spaceCount;
            }
        }
        private int GetSpace(string beforeString) {
            return spaceCount - beforeString.Length;
        }

    }
}
