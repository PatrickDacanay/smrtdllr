using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmartDollarWorker.Domains.Interfaces;
using SmartDollarWorker.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDollarWorker.Access.DbApps
{
    public class SmartDollarApp : DbBase, ISmartDollar
    {
        public SmartDollarApp(IConfiguration configs) : base(configs.GetConnectionString("DbSmartDollar") ?? "")
        {
        }
        public SmrtDllrCnst GetTransactions()
        {
            var listdata = new SmrtDllrCnst();
            var connection = GetConnections();
            long dateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            try
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    var CountTicket = MySqlHelper.ExecuteScalar(connection,$"SELECT COUNT(TICKET) FROM fm_db.MT4_TRADES;").ToString();
                    var storedTicket = int.Parse(CountTicket);
                    var updateTicekt = MySqlHelper.ExecuteScalar(connection, $"UPDATE smatdollar.configuration SET value = {storedTicket} WHERE (id = 1);");

                    cmd.CommandText = $"SELECT tr.ticket, tr.login, 0 as status, {dateTime} as date_added, {dateTime} date_modified FROM smatdollar.accounts sd" +
                        $"join fm_db.MT4_TRADES tr on tr.LOGIN = sd.LOGIN join smatdollar.symbols s on s.symbol = tr.SYMBOL WHERE TICKET > (SELECT value FROM " +
                        $"smatdollar.configuration where id = 1) ORDER BY ticket DESC LIMIT 1000;";
                    using (var readData = cmd.ExecuteReader())
                    {
                        while ( readData.Read())
                        {
                            listdata = new SmrtDllrCnst()
                            {
                                Ticket = int.Parse(readData["ticket"].ToString() ?? ""),
                                Login = int.Parse(readData["login"].ToString() ?? ""),
                                Status = int.Parse(readData["status"].ToString() ?? ""),
                                Date_added = int.Parse(readData["date_added"].ToString() ?? ""),
                                Date_modified = int.Parse(readData["date_modified"].ToString() ?? "")
                            };
                        }
                    }
                }
                return listdata;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void PostSmartDollar(SmrtDllrCnst request)
        {
            var connection = GetConnections();
            try
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO smatdollar.transactions (ticket,login,status,date_added,date_modified) VALUES " +
                                          $"({request.Ticket},{request.Login},{request.Status},{request.Date_added},{request.Date_modified});";
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close() ;
                    connection.Dispose();
                }
            }
        }
    }
}
