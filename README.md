# CryptoWall3Finder
Console application for scanning a file system for all files encrypted by CryptoWall 3.0.

## Why?
To recover from a CW3 attack without paying the ransom, we ended up with a mixed set of
files that Volume Shadow Copy was able to restore, files that we had good backups for,
and files where the backup file was also encrypted. We needed a way to get a sure list
of all the files that were still encrypted.

It turns out every CW3-encrypted file on your PC starts with the same 16-byte sequence.
This sequence is unique to your PC, but every encrypted file has it. This program uses
that 16-byte sequence and does a recursive file search to list all encrypted files.

## How?
At the moment I'm just throwing this out to see if anyone else is interested. If you
need this tool and need a .exe that you can run, let me know and I'll add some code
to make this tool more approachable.

  1. Get the source.
  2. Edit Program.cs on line 44-49, setting _evilBytes to your PC's unique 16-byte sequence.
  3. Compile and run: cw3find.exe c:\folder\to\scan
  4. After the scan is completed, evil.txt is written to the current directory with the list.

If you need additional help, add an issue to this project and I'll try to help out!
