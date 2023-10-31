using System;

class Quaternion
{
    public double W { get; private set; }
    public double X { get; private set; }
    public double Y { get; private set; }
    public double Z { get; private set; }

    public Quaternion(double w, double x, double y, double z)
    {
        W = w;
        X = x;
        Y = y;
        Z = z;
    }

    // Перегрузка операторів для додавання кватерніонів
    public static Quaternion operator +(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.W + q2.W, q1.X + q2.X, q1.Y + q2.Y, q1.Z + q2.Z);
    }

    // Перегрузка операторів для віднімання кватерніонів
    public static Quaternion operator -(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.W - q2.W, q1.X - q2.X, q1.Y - q2.Y, q1.Z - q2.Z);
    }

    // Перегрузка операторів для множення кватерніонів
    public static Quaternion operator *(Quaternion q1, Quaternion q2)
    {
        double w = q1.W * q2.W - q1.X * q2.X - q1.Y * q2.Y - q1.Z * q2.Z;
        double x = q1.W * q2.X + q1.X * q2.W + q1.Y * q2.Z - q1.Z * q2.Y;
        double y = q1.W * q2.Y - q1.X * q2.Z + q1.Y * q2.W + q1.Z * q2.X;
        double z = q1.W * q2.Z + q1.X * q2.Y - q1.Y * q2.X + q1.Z * q2.W;
        return new Quaternion(w, x, y, z);
    }

    // Метод для обчислення норми кватерніона
    public double Norm()
    {
        return Math.Sqrt(W * W + X * X + Y * Y + Z * Z);
    }

    // Метод для обчислення спряженого кватерніона
    public Quaternion Conjugate()
    {
        return new Quaternion(W, -X, -Y, -Z);
    }

    // Метод для обчислення інверсного кватерніона
    public Quaternion Inverse()
    {
        double norm = Norm();
        double normSquared = norm * norm;

        if (normSquared == 0)
        {
            throw new InvalidOperationException("Cannot invert a quaternion with zero norm.");
        }

        Quaternion conjugate = Conjugate();
        return new Quaternion(conjugate.W / normSquared, conjugate.X / normSquared, conjugate.Y / normSquared, conjugate.Z / normSquared);
    }

    // Перегрузка операторів для порівняння кватерніонів
    public static bool operator ==(Quaternion q1, Quaternion q2)
    {
        return q1.W == q2.W && q1.X == q2.X && q1.Y == q2.Y && q1.Z == q2.Z;
    }

    public static bool operator !=(Quaternion q1, Quaternion q2)
    {
        return !(q1 == q2);
    }

    // Метод для конвертації кватерніона в матрицю обертання
    public double[,] ToRotationMatrix()
    {
        double[,] matrix = new double[3, 3];

        double xx = X * X;
        double xy = X * Y;
        double xz = X * Z;
        double xw = X * W;
        double yy = Y * Y;
        double yz = Y * Z;
        double yw = Y * W;
        double zz = Z * Z;
        double zw = Z * W;

        matrix[0, 0] = 1 - 2 * (yy + zz);
        matrix[0, 1] = 2 * (xy - zw);
        matrix[0, 2] = 2 * (xz + yw);
        matrix[1, 0] = 2 * (xy + zw);
        matrix[1, 1] = 1 - 2 * (xx + zz);
        matrix[1, 2] = 2 * (yz - xw);
        matrix[2, 0] = 2 * (xz - yw);
        matrix[2, 1] = 2 * (yz + xw);
        matrix[2, 2] = 1 - 2 * (xx + yy);

        return matrix;
    }
}

class Program
{
    static void Main()
    {
        Quaternion q1 = new Quaternion(1, 2, 3, 4);
        Quaternion q2 = new Quaternion(5, 6, 7, 8);

        Quaternion addition = q1 + q2;
        Quaternion subtraction = q1 - q2;
        Quaternion multiplication = q1 * q2;

        Console.WriteLine("Addition: " + addition.W + " " + addition.X + " " + addition.Y + " " + addition.Z);
        Console.WriteLine("Subtraction: " + subtraction.W + " " + subtraction.X + " " + subtraction.Y + " " + subtraction.Z);
        Console.WriteLine("Multiplication: " + multiplication.W + " " + multiplication.X + " " + multiplication.Y + " " + multiplication.Z);

        Console.WriteLine("Norm of q1: " + q1.Norm());
        Console.WriteLine("Conjugate of q1: " + q1.Conjugate().W + " " + q1.Conjugate().X + " " + q1.Conjugate().Y + " " + q1.Conjugate().Z);
        Console.WriteLine("Inverse of q1: " + q1.Inverse().W + " " + q1.Inverse().X + " " + q1.Inverse().Y + " " + q1.Inverse().Z);

        double[,] rotationMatrix = q1.ToRotationMatrix();
        Console.WriteLine("Rotation Matrix:");
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(rotationMatrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}
