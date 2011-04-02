#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <ctype.h>

//	funkcije za ispisivanje na ekran
void start_screen ();
void screen_imp ();
void screen_exp1 ();
void screen_exp2 ();
void screen_exp3 ();
void exit_screen ();
void help_screen ();

//	funkcije za citanje iz datoteka
int import_main (int Ty);			//glavna fja

int import_mpc (int N);
int import_skymap (int N);
int import_guide (int N);
int import_xephem (int N);
int import_voyager (int N);
int import_home_planet (int N);
int import_mystars (int N);			//comet[i].pn zapravo nije .pn !!!
int import_thesky (int N);
int import_starry_night (int N);
int import_deep_space (int N);
int import_pc_tcs (int N);
int import_skytools (int N);
int import_skychart (int N);
int import_ecu (int N);
int import_dance (int N);
int import_megastar (int N);
int import_cfw (int N);				//comet for windows
int import_nasa1 (int N);			//ELEMENTS.COMET
int import_nasa2 (int N);			//sbdb query

//	funkcije za pisanje u datoteke
void export_ssc (int N, int Ty);
void export_stell (int N, int Ty);
void export_mpc (int N, int Ty);
void export_skymap (int N, int Ty);
void export_home_planet (int N, int Ty);
void export_mystars (int N, int Ty);
void export_thesky (int N, int Ty);
void export_ecu (int N, int Ty);
void export_skychart (int N, int Ty);
void export_skytools (int N, int Ty);

//	funkcije za racunanje...
double compute_period (float q, float e);
long int compute_JD (int y, int m, int d);
char *edit_name (char *name);
