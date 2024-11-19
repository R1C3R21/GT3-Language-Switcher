# Gran Turismo 3 Language Switcher
Program that lets the user change the language of a save file for the PAL version of Gran Turismo 3 (SCES-50294).

### Requirements
  * Save file in *raw format* (in PCSX2, the memory card has to be formatted as a **folder**)

### How to use
1. First, open the save file, which is generally called `BESCES-50294GAMEDATA`.
2. Once opened, the program will autodetect the language for the save offered.
3. Select the language you wish to change to, and press Save.

### Addendum
While the code also documents how the save file is structured (to the best of my knowledge, since saves tend to vary greatly between themselves, and thus, getting patterns down can be hard at times), here is the same information for accessibility purposes.
The file is divided into 3 parts (sometimes only 2):
  * Header (64 bytes): ??? (12 bytes) + CRC32 value of the raw data section (4 bytes) + Filler bytes (48 bytes)
  * Raw data: the meat and potatoes of the save, that's where the garage and everything regarding the profile's progress is stored at
  * Footer: this section appears once the last byte of the raw data ends, which is always set at FF. This part sometimes is missing from the save files, so its purpose is yet to be understood.