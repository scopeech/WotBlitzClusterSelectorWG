using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace wotblitzclusterselector
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> servers = new Dictionary<string, string>()
        {
            { "EU_C0 (Нидерланды)", "login0.wotblitz.eu" },
            { "EU_C1 (Нидерланды)", "login1.wotblitz.eu" },
            { "EU_C2 (Германия)", "login2.wotblitz.eu" },
            { "EU_C3 (Польша)", "login3.wotblitz.eu" },
            { "EU_C4 (Казахстан)", "login4.wotblitz.eu" }
        };

        private string replacementIp = "0.0.0.0";
        private string hostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";

        public Form1()
        {
            InitializeComponent();
        }

        private void Log(string message)
        {
            txtConsoleOutput.AppendText($"{DateTime.Now:T} — {message}{Environment.NewLine}");
        }

        private void BlockServer(string hostname)
        {
            var lines = File.ReadAllLines(hostsFilePath);
            if (!lines.Any(line => line.Contains(hostname)))
            {
                File.AppendAllText(hostsFilePath, $"{replacementIp} {hostname}{Environment.NewLine}");
                Log($"Сервер {hostname} заблокирован.");
            }
            else
            {
                Log($"Сервер {hostname} уже заблокирован.");
            }
        }

        private void UnblockAllServers()
        {
            var lines = File.ReadAllLines(hostsFilePath);
            var newLines = new List<string>();
            foreach (var line in lines)
            {
                if (!servers.Values.Any(host => line.Contains(host)))
                {
                    newLines.Add(line);
                }
            }
            File.WriteAllLines(hostsFilePath, newLines);
            Log("Все серверы разблокированы.");
        }

        private async void ShowPings()
        {
            Ping pingSender = new Ping();
            foreach (var pair in servers)
            {
                try
                {
                    PingReply reply = await pingSender.SendPingAsync(pair.Value, 1000);
                    if (reply.Status == IPStatus.Success)
                    {
                        Log($"{pair.Key} — {reply.RoundtripTime} ms");
                    }
                    else
                    {
                        Log($"{pair.Key} — Не отвечает");
                    }
                }
                catch (Exception ex)
                {
                    Log($"{pair.Key} — Ошибка: {ex.Message}");
                }
            }
        }

        private void ShowCurrentHosts()
        {
            try
            {
                string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";
                System.Diagnostics.Process.Start("notepad.exe", hostsPath);
                Log("Открытие файла hosts прошло успешно.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть файл hosts:\n" + ex.Message);
            }
        }

        private void btnBlockC0_Click_Click(object sender, EventArgs e)
        {
            BlockServer("login0.wotblitz.eu");
        }

        private void btnBlockC1_Click_Click(object sender, EventArgs e)
        {
            BlockServer("login1.wotblitz.eu");
        }

        private void btnBlockC2_Click_Click(object sender, EventArgs e)
        {
            BlockServer("login2.wotblitz.eu");
        }

        private void btnBlockC3_Click_Click(object sender, EventArgs e)
        {
            BlockServer("login3.wotblitz.eu");
        }

        private void btnBlockC4_Click_Click(object sender, EventArgs e)
        {
            BlockServer("login4.wotblitz.eu");
        }

        private void btnUnblockAll_Click_Click(object sender, EventArgs e)
        {
            UnblockAllServers();
        }

        private void btnShowPings_Click_Click(object sender, EventArgs e)
        {
            ShowPings();
        }

        private void btnShowHosts_Click_Click(object sender, EventArgs e)
        {
            ShowCurrentHosts();
        }
        private void txtConsoleOutput_TextChanged(object sender, EventArgs e)
        {
            // Можно оставить пустым или добавить лог, если нужно
        }

    }
}
