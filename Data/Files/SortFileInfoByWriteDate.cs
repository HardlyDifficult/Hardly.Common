using System;
using System.IO;
using System.Collections.Generic;

namespace HD
{
  public class SortFileInfoByWriteDate : IComparer<FileInfo>
  {
    int IComparer<FileInfo>.Compare(FileInfo x, FileInfo y)
    {
      if(x.LastWriteTimeUtc == y.LastWriteTimeUtc)
      {
        return 0;
      }

      return x.LastWriteTimeUtc > y.LastWriteTimeUtc ? -1 : 1;
    }
  }
}
