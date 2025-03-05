namespace CrapCord_Services;

using System;
using System.Diagnostics;

public class HardwareIdentifier
{
    private Process _process;
    private StreamWriter _inputStream;
    private StreamReader _outputStream;

    public string CpuId { get; private set; }
    public string MotherboardSerial { get; private set; }

    public HardwareIdentifier()
    {
        StartSuperUserShell();
        GetHardwareInfo();
        CloseShell();
    }

    private void StartSuperUserShell()
    {
        try
        {
            // Launch pkexec to start su with an interactive shell
            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "pkexec",
                    Arguments = "su",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            _process.Start();

            _inputStream = _process.StandardInput;
            _outputStream = _process.StandardOutput;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error starting superuser shell: " + ex.Message);
        }
    }

    private void GetHardwareInfo()
    {
        if (_inputStream == null || _outputStream == null)
        {
            throw new InvalidOperationException("Superuser shell not started.");
        }

        try
        {
            // Get CPU ID
            _inputStream.WriteLine("dmidecode -s system-uuid");
            _inputStream.Flush();
            CpuId = _outputStream.ReadLine();

            // Get Motherboard Serial
            _inputStream.WriteLine("dmidecode -s baseboard-serial-number");
            _inputStream.Flush();
            MotherboardSerial = _outputStream.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error executing command: " + ex.Message);
        }
    }

    private void CloseShell()
    {
        try
        {
            // Close the shell session
            _inputStream?.WriteLine("exit");
            _inputStream?.Flush();
            _process?.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error closing superuser shell: " + ex.Message);
        }
    }
}
