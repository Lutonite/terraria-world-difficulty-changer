// TNoob - Trivial tool for changing the "Expert Mode" flag of a world
using System;
using System.IO;
using System.Windows;

public static class Program {
    public static void Main(string[] args) {
        if (args.Length < 1) {
            Console.Out.WriteLine("Drag and drop the world file to the executable!");
            Console.ReadKey();
            return;
        }

        int difficulty = -1;
        Console.WriteLine("Enter Game Mode setting (normal - 0, expert - 1, master - 2, creative - 3):");
        while (difficulty < 0)
        {
            string difficultyInput = Console.ReadLine();
            if (!int.TryParse(difficultyInput, out difficulty))
            {
                try
                {
                    switch (difficultyInput.ToLower())
                    {
                        case "normal":
                            difficulty = 0;
                            break;
                        case "expert":
                            difficulty = 1;
                            break;
                        case "master":
                            difficulty = 2;
                            break;
                        case "creative":
                            difficulty = 3;
                            break;
                        default:
                            Console.Error.WriteLine(
                                "Invalid input, please try again. (normal - 0, expert - 1, master - 2, creative - 3):");
                            break;
                    }
                }
                catch (NullReferenceException)
                {}
            }
        }

        bool hardMode = false;
        bool hardModeCheck = false;
        Console.WriteLine("Turn on Hard Mode? (true, false):");
        while (!hardModeCheck)
        {
            string hardModeInput = Console.ReadLine();
            if (Boolean.TryParse(hardModeInput, out hardMode))
            {
                hardModeCheck = true;
            }
            else
            {
                Console.Error.WriteLine("Invalid input, please try again. (true, false):");
            }
        }


        SetExpertMode(args[0], args[0], difficulty, hardMode);
    }

    private static void SetExpertMode(string source, string dest, int difficulty, bool hardMode)
    {
        Console.Out.WriteLine("Difficulty: {0} | Hard Mode: {1}", difficulty, hardMode);
        
        BinaryReader reader = new BinaryReader(new FileStream(source, FileMode.Open));
        int version = reader.ReadInt32();
        if (version < 225) {
            Console.Out.WriteLine("Error: Outdated terraria version. This tool is made for curVersion >= 225 (Terraria 1.4).");
            Console.ReadKey();
            return;
        }
        ulong magic = reader.ReadUInt64();
        if ((magic & 72057594037927935uL) != 27981915666277746uL) {
            Console.Out.WriteLine("Error: Invalid header");
            Console.ReadKey();
            return;
        }
        // Skip other file metadata...
        reader.ReadUInt32();
        reader.ReadUInt64();

        reader.ReadInt16();
        
        reader.BaseStream.Position = reader.ReadInt32();
        reader.ReadString();
        reader.ReadString();
        reader.ReadUInt64();

        reader.ReadBytes(16);
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();

        long gameModeOffset = reader.BaseStream.Position;
        int gameMode = reader.ReadInt32();
        bool drunkWorld = reader.ReadBoolean();

        reader.ReadInt64();
        reader.ReadByte();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadDouble();
        reader.ReadDouble();
        reader.ReadDouble();
        reader.ReadBoolean();
        reader.ReadInt32();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadInt32();
        reader.ReadInt32();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadBoolean();
        reader.ReadByte();
        reader.ReadInt32();

        long hardModeOffset = reader.BaseStream.Position;
        bool isHardMode = reader.ReadBoolean();

        reader.Dispose();

        BinaryWriter writer = new BinaryWriter(new FileStream(dest, FileMode.Open));
        writer.BaseStream.Position = gameModeOffset;
        writer.Write(difficulty);
        writer.BaseStream.Position = hardModeOffset;
        writer.Write(hardMode);
        writer.Dispose();
        
        Console.Out.WriteLine("Save game updated!");
        Console.ReadKey();
    }
}
