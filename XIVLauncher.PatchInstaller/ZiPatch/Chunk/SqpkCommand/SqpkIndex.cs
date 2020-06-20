﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XIVLauncher.PatchInstaller.Util;
using XIVLauncher.PatchInstaller.ZiPatch.Util;

namespace XIVLauncher.PatchInstaller.ZiPatch.Chunk.SqpkCommand
{
    class SqpkIndex : SqpkChunk
    {
        // This is a NOP on recent patcher versions.
        public new static string Command = "I";

        public enum IndexCommandKind : byte
        {
            Add = (byte)'A',
            Delete = (byte)'D'
        }

        public IndexCommandKind IndexCommand { get; protected set; }
        public bool IsSynonym { get; protected set; }
        public SqpackIndexFile TargetFile { get; protected set; }
        public ulong FileHash { get; protected set; }
        public uint BlockOffset { get; protected set; }

        // TODO: Figure out what this is used for
        public uint BlockNumber { get; protected set; }



        public SqpkIndex(ChecksumBinaryReader reader, int size) : base(reader, size) {}

        protected override void ReadChunk()
        {
            var start = reader.BaseStream.Position;

            IndexCommand = (IndexCommandKind)reader.ReadByte();
            IsSynonym = reader.ReadBoolean();
            reader.ReadByte(); // Alignment

            TargetFile = new SqpackIndexFile(reader);

            FileHash = reader.ReadUInt64BE();

            BlockOffset = reader.ReadUInt32BE();
            BlockNumber = reader.ReadUInt32BE();

            reader.ReadBytes(Size - (int)(reader.BaseStream.Position - start));
        }

        public override string ToString()
        {
            return $"{Type}:{Command}:{IndexCommand}:{IsSynonym}:{TargetFile}:{FileHash:X8}:{BlockOffset}:{BlockNumber}";
        }
    }
}
