using System.Drawing;
using Colorful;

namespace Hazebroek.Tgtg.Infra
{
    public abstract class ConsolePrinter
    {
        public abstract void WriteLine();

        public abstract void WriteLine(string value);

        public abstract void WriteLineFormatted(
            string format,
            Color defaultColor,
            params Formatter[] args
        );

        public abstract void Write(string value);

        public abstract void WriteLine(string value, in Color color);
    }

    public class CliConsolePrinter : ConsolePrinter
    {
        public override void WriteLine()
        {
            Console.WriteLine();
        }

        public override void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public override void WriteLineFormatted(string format, Color defaultColor, params Formatter[] args)
        {
            Console.WriteLineFormatted(format, defaultColor, args);
        }

        public override void Write(string value)
        {
            Console.Write(value);
        }

        public override void WriteLine(string value, in Color color)
        {
            System.Console.WriteLine(value, color);
        }
    }

    public class WorkerConsolePrinter : ConsolePrinter
    {
        public override void WriteLine()
        {
        }

        public override void WriteLine(string value)
        {
        }

        public override void WriteLineFormatted(string format, Color defaultColor, params Formatter[] args)
        {
        }

        public override void Write(string value)
        {
        }

        public override void WriteLine(string value, in Color color)
        {
        }
    }
}