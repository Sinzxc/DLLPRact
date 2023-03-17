using System;
using System.Data.Common;
using System.Security.Cryptography;

namespace PolygonLib
{
    public class Point:IComparable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point()
        {
            X = 0;
            Y = 0;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public static int operator *(Point a, Point b)
        => a.X * b.X +a.Y*b.Y;

        public int CompareTo(object? obj)
        {
            throw new NotImplementedException();
        }
    }

    public class Rectangle:Point
    {
        Point[] points = new Point[4];
        Double ABC, BCD, CDA, DAB;
        double AB, BC, CD, DA, AC, BD;
        double S, P;
        Random rand = new Random();
        public Rectangle()
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Point(rand.Next(10), rand.Next(10));
            }
        }
        public Rectangle(Point p1, Point p2, Point p3, Point p4)
        {
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            points[3] = p4;
        }

        public double Angles(Point a, Point b,Point c)
        {
            Point AB =new Point(a.X-b.X,a.Y-b.Y);
            Point AC = new Point(a.X - c.X, a.Y - b.Y);
            double Angle = (AB * AC) / (Math.Abs(AB.X * AB.X + AB.Y * AB.Y)* Math.Abs(AC.X * AC.X + AC.Y * AC.Y));
            return Angle;
        }
        public void SearchAngles()
        {
            ABC = Angles(points[0], points[1], points[2]);
            BCD = Angles(points[1], points[2], points[3]);
            CDA = Angles(points[2], points[3], points[0]);
            DAB = Angles(points[3], points[0], points[1]);
        }

        public void SearchSides()
        {
            AB = Math.Sqrt((points[1].X - points[0].X) * (points[1].X - points[0].X) + (points[1].Y - points[0].Y) * (points[1].Y - points[0].Y));
            BC = Math.Sqrt((points[2].X - points[1].X) * (points[2].X - points[1].X) + (points[2].Y - points[1].Y) * (points[2].Y - points[1].Y));
            CD = Math.Sqrt((points[3].X - points[2].X) * (points[3].X - points[2].X) + (points[3].Y - points[2].Y) * (points[3].Y - points[2].Y));
            DA = Math.Sqrt((points[0].X - points[3].X) * (points[0].X - points[3].X) + (points[0].Y - points[3].Y) * (points[0].Y - points[3].Y));
        }
        public virtual void SearchSquad()
        {
            SearchPerimetr();
            double pp = P / 2;
            S = Math.Sqrt((pp - AB) * (pp - BC) * (pp - CD) * (pp - DA));
        }

        public void SearchPerimetr()
        {
            double P = (AB + BC + CD + DA);
        }

        public void SearchDiagonals()
        {
            AC = Math.Sqrt((points[2].X - points[0].X) * (points[2].X - points[0].X) + (points[2].Y - points[0].Y) * (points[2].Y - points[0].Y));
            BD = Math.Sqrt((points[3].X - points[1].X) * (points[3].X - points[1].X) + (points[3].Y - points[1].Y) * (points[3].Y - points[1].Y));
        }
        public override string ToString()
        {
            return $"\n({points[0]},{points[1]},{points[2]},{points[3]})\nПлощадь:{S}\nПериметр:{P}\nСтороны:\nAB={AB}, BC={BC}, CD={CD}, DA={DA}\nДиагонали:{AC}, {BD}\n Углы:\nABC={ABC}, BCD={BCD}, CDA={CDA}, DAB={DAB}";
        }
        public override bool Equals(object p)
        {
            Convex temp = (Convex)p;
            if (this.S == temp.S)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
            return base.GetHashCode();
        }
    }

    public class Convex:Rectangle
    {
        Point[] points = new Point[4];
        Double ABC, BCD, CDA, DAB;
        double AB, BC, CD, DA,AC,BD;
        double S,P;
        public Convex() : base() { SearchAngles(); SearchSides();SearchSquad(); }
        public Convex(Point p1, Point p2, Point p3, Point p4) : base(p1,p2,p3,p4) { SearchAngles(); SearchSides(); SearchSquad(); }
        public override string ToString()
        {
            return "Выпуклый четырёхуг:\n"+ base.ToString();
        }
    }
    public class Parallelogramm:Rectangle
    {
        Point[] points = new Point[4];
        Double ABC, BCD, CDA, DAB;
        double AB, BC, CD, DA, AC, BD;
        double S, P;
        Parallelogramm() : base() { SearchAngles(); SearchSides(); SearchSquad(); }
        Parallelogramm(Point p1, Point p2, Point p3, Point p4) : base(p1, p2, p3, p4) { SearchAngles(); SearchSides(); SearchSquad(); }

        public override void SearchSquad()
        {
            double h = CDA * DA;
            S = CD * h;
        }
        public override string ToString()
        {
            return "Параллелограмм:\n" + base.ToString();
        }
    }

    public class Rhombus:Rectangle
    {
        Point[] points = new Point[4];
        Double ABC, BCD, CDA, DAB;
        double AB, BC, CD, DA, AC, BD;
        double S, P;
        public Rhombus() : base() { SearchAngles(); SearchSides(); SearchSquad(); }
        public Rhombus(Point p1, Point p2, Point p3, Point p4) : base(p1, p2, p3, p4) { SearchAngles(); SearchSides(); SearchSquad(); }

        public override void SearchSquad()
        {
            double h = (AC * BD )/( 2 * CD);
            S = CD * h;
        }
        public override string ToString()
        {
            return "Ромб:\n" + base.ToString();
        }
    }
    public class Squar:Rectangle
    {
        Point[] points = new Point[4];
        Double ABC, BCD, CDA, DAB;
        double AB, BC, CD, DA, AC, BD;
        double S, P;
        public Squar() : base() { SearchAngles(); SearchSides(); SearchSquad(); }
        public Squar(Point p1, Point p2, Point p3, Point p4) : base(p1, p2, p3, p4) { SearchAngles(); SearchSides(); SearchSquad(); }

        public override void SearchSquad()
        {
            S = CD * 4;
        }
        public override string ToString()
        {
            return "Квадрат:\n" + base.ToString();
        }
    }

}

