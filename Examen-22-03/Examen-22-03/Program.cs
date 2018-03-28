using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<string> write = Console.Write;
            Func<string> read = Console.ReadLine;
            Action clear = Console.Clear;
            var paint = new Paint(write, read, clear);
            var color = new Color();
            var shape = new Shape();

            do
            {
                try
                {
                    var opt = paint.init();
                    paint.execution(opt, color, shape);
                }
                catch (Exception e)
                {
                    write(e.Message);
                    read();
                }
            }
            while (!paint.exit);
        }
    }

    public enum EnumColor
    {
        Rojo,
        Azul,
        Verde,
        Amarillo,
        Rosa,
        Negro,
        Marron,
        Blanco
    }

    public enum EnumShape
    {
        Corazon,
        Rayo,
        Estrella,
        Cuadrado
    }

    public class Paint
    {
        readonly Action<string> _write;
        readonly Func<string> _read;
        readonly Action _clear;
        public bool exit = false;
        public Canvas canvas = Canvas.Instance;

        public Paint(Action<string> write, Func<string> read, Action clear)
        {
            _write = write;
            _read = read;
            _clear = clear;
        }
        public string init()
        {
            _clear();
            _write("1. Pintar figuras.\n");
            _write("2. Mostrar figuras.\n");
            _write("3. Salir.\n");
            return _read();
        }
        public void execution(string a, IColor color, IShape shape)
        {
            _clear();
            switch (a)
            {
                case "1":
                    {
                        var figures = canvas.Figures;
                        _write("Inserte figura: ");
                        var s = _read();
                        _write("Inserte Color de Borde: ");
                        var b = _read();
                        _write("Inserte Color de Relleno: ");
                        var r = _read();

                        var fig = new Figure(shape.GetShape(s), color.GetColor(b), color.GetColor(r));
                        figures.Add(fig);
                        canvas.Figures = figures;
                        break;
                    }
                case "2":
                    {
                        var printer = new Print();
                        _write(printer.Printer(canvas.Figures));
                        _read();
                        break;
                    }
                case "3":
                    {
                        this.exit = true;
                        break;
                    }
            }
        }
    }

    public interface IColor
    {
        EnumColor GetColor(string color);
    }

    public class Color : IColor
    {
        public EnumColor GetColor(string color)
        {
            EnumColor convertColor;
            if (!Enum.TryParse(color, out convertColor))
            {
                throw new Exception(string.Format("el color {0} no existe.", color));
            }
            return convertColor;
        }
    }

    public interface IShape
    {
        EnumShape GetShape(string shape);
    }

    public class Shape : IShape
    {
        public EnumShape GetShape(string shape)
        {
            EnumShape convertShape;
            if (!Enum.TryParse(shape, out convertShape))
            {
                throw new Exception(string.Format("La forma {0} no existe.", shape));
            }
            return convertShape;
        }
    }

    public class Canvas
    {
        private static Canvas instance = null;
        protected Canvas() { }

        public static Canvas Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Canvas();
                    return instance;
                }
                throw new Exception(string.Format("Ya hay un Lienzo invocado."));
            }
        }

        public List<IFigure> Figures = new List<IFigure>(); //Figuras que hay en el lienzo.
    }

    public interface IFigure
    {
        EnumShape Shape { get; set; }
        EnumColor FillColor { get; set; }
        EnumColor BorderColor { get; set; }
    }

    public class Figure : IFigure
    {
        public EnumShape Shape { get; set; }
        public EnumColor FillColor { get; set; }
        public EnumColor BorderColor { get; set; }

        public Figure(EnumShape s, EnumColor f = EnumColor.Blanco, EnumColor b = EnumColor.Negro)
        {
            Shape = s;
            FillColor = f;
            BorderColor = b;
        }
    }

    public interface IPrint
    {
        string Printer(IList<IFigure> list);
    }

    public class Print : IPrint
    {
        StringBuilder str = new StringBuilder();
        public string Printer(IList<IFigure> list)
        {
            foreach (var item in list)
            {
                var forma = item.Shape.ToString();
                var colorB = item.BorderColor.ToString();
                var colorR = item.FillColor.ToString();

                str.Append(forma);
                str.Append(", de relleno ");
                str.Append(colorB);
                str.Append(" y borde ");
                str.Append(colorR);
                str.Append("\n");
            }
            return str.ToString();
        }
    }
}
