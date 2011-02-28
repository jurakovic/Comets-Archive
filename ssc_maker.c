// Celestia and Stellarium Format Maker

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <ctype.h>
#define MAX 5000


//	funkcije za ispisivanje na ekran
void initscreen1();
void initscreen2();
void screen_imp();
void screen_exp1();
void screen_exp2();
void screen_exp3();


//	funkcije za citanje iz datoteka
int  import_menu();				//glavna fja
void import_mpc();
void import_skymap();
void import_guide();
void import_xephem();
void import_voyager();
void import_home_planet();
void import_mystars();			//comet[i].pn zapravo nije .pn !!!
void import_thesky();
void import_starry_night();
void import_deep_space();
void import_pc_tcs();
void import_skytools();
void import_skychart();
void import_ecu();
void import_dance();
void import_megastar();
void import_nasa1();			//ELEMENTS.COMET
void import_nasa2();			//sbdb query
void import_cfw();				//comet for windows


//	funkcije za racunanje...
float compute_period (float q, float e);
int   compute_JD (int y, int m, int d);
char *edit_name (char *name);


//	funkcije za pisanje u datoteke
void output_ssc ();
void output_stell ();


//	globalne varijable
int   type;
char *soft;
char *input_format;
char *output_format;
int   Ncmt;
char  fin_name[80+1];
char  fout_name[80+1];

struct cmt{
	char name [80+1];
	char ID [16+1];
	int JD;
	int y;
	int m;
	int d;
	int h;
	int eq;
	float q;
	float e;
	float pn;
	float an;
	float i;
	float H;
	float G;
	float P;
	char x [80+1];
} comet[MAX];


int main (){

	int a;

	system("COLOR 9");

	initscreen2();
	getch();
	system("CLS");

	start:
	fflush(stdin);
	fflush(stdout);
	system("CLS");
	printf("\n");
	printf("                      CELESTIA AND STELLARIUM FORMAT MAKER\n\n");
	printf(" ==============================================================================\n\n");
	printf("  Supported input formats: \n\n");
	printf("        0. MPC                         12. MegaStar V4.x\n");
	printf("        1. SkyMap                      13. SkyChart III\n");
	printf("        2. Guide                       14. Voyager II\n");
	printf("        3. xephem                      15. SkyTools\n");
	printf("        4. Home Planet                 16. Autostar\n");
	printf("        5. MyStars!\n");
	printf("        6. TheSky                      17. Comet for Windows\n");
	printf("        7. Starry Night                18. NASA (ELEMENTS.COMET)\n");
	printf("        8. Deep Space                  19. NASA (CSV format)\n");
	printf("        9. PC-TCS\n");
	printf("       10. Earth Centered Universe     20. Help\n");
	printf("       11. Dance of the Planets        21. Exit\n\n");
	printf("  Select option [0-21]: ");

	scanf("%d", &type);

	switch (type){

		case 0: {
			input_format = "MPC";
			soft = "Soft00Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 1: {
			input_format = "SkyMap";
			soft == "Soft01Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 2: {
			input_format = "Guide";
			soft = "Soft02Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 3: {
			input_format = "xephem";
			soft = "Soft03Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 4: {
			input_format = "Home Planet";
			soft = "Soft04Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 5: {
			input_format = "MyStars!";
			soft = "Soft05Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 6: {
			input_format = "TheSky";
			soft = "Soft06Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 7: {
			input_format = "Starry Night";
			soft = "Soft07Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 8: {
			input_format = "Deep Space";
			soft = "Soft08Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 9: {
			input_format = "PC-TCS";
			soft = "Soft09Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 10: {
			input_format = "Earth Centered Universe";
			soft = "Soft10Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 11: {
			input_format = "Dance of the Planets";
			soft = "Soft11Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 12: {
			input_format = "MegaStar V4.x";
			soft = "Soft12Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 13: {
			input_format = "SkyChart III";
			soft = "Soft13Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 14: {
			input_format = "Voyager II";
			soft = "Soft14Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 15: {
			input_format = "SkyTools";
			soft = "Soft15Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 16: {
			input_format = "Autostar";
			soft = "Soft16Cmt";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 17: {
			input_format = "Comet for Windows";
			soft = "Comet.dat";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 18: {
			input_format = "NASA";
			soft = "ELEMENTS.COMET";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 19: {
			input_format = "NASA";
			soft = "CSV format";
			a=import_menu();
			if (a==1) goto start;
			if (a==2) goto end;
		}

		case 21: goto end;

		default: goto start;
	}

	end:
	initscreen2();
	printf("Press any key to exit...                             Copyright (c) 2011, jurluk");
	getch();
	return 0;
}


void initscreen1(){

	fflush(stdin);
	fflush(stdout);
	system("CLS");
	printf("\n\n\n\n");
	printf("                _   _   _   _   _   _   _   _       _   _   _  \n");
	printf("               / \\ / \\ / \\ / \\ / \\ / \\ / \\ / \\     / \\ / \\ / \\ \n");
	printf("              ( C ) e ) l ) e ) s ) t ) i ) a )   ( a ) n ) d )\n");
	printf("               \\_/ \\_/ \\_/ \\_/ \\_/ \\_/ \\_/ \\_/     \\_/ \\_/ \\_/ \n");
	printf("                    _   _   _   _   _   _   _   _   _   _  \n");
	printf("                   / \\ / \\ / \\ / \\ / \\ / \\ / \\ / \\ / \\ / \\ \n");
	printf("                  ( S ) t ) e ) l ) l ) a ) r ) i ) u ) m )\n");
	printf("                   \\_/ \\_/ \\_/ \\_/ \\_/ \\_/ \\_/ \\_/ \\_/ \\_/ \n");
	printf("                _   _   _   _   _   _       _   _   _   _   _  \n");
	printf("               / \\ / \\ / \\ / \\ / \\ / \\     / \\ / \\ / \\ / \\ / \\ \n");
	printf("              ( F ) o ) r ) m ) a ) t )   ( M ) a ) k ) e ) r )\n");
	printf("               \\_/ \\_/ \\_/ \\_/ \\_/ \\_/     \\_/ \\_/ \\_/ \\_/ \\_/ \n\n\n\n\n\n\n\n\n");
}

void initscreen2(){

	fflush(stdin);
	fflush(stdout);
	system("CLS");
	printf("\n\n");
	printf("             ____     _          _   _                          _ \n");
	printf("            / ___|___| | ___ ___| |_(_) __ _     __ _ _ __   __| |\n");
	printf("           | |   / _ \\ |/ _ \\ __| __| |/ _` |   / _` | '_ \\ / _` |\n");
	printf("           | |___  __/ |  __\\__ \\ |_| | (_| |  | (_| | | | | (_| |\n");
	printf("            \\____\\___|_|\\___|___/\\__|_|\\__,_|   \\__,_|_| |_|\\__,_|\n\n");
	printf("                ____  _       _ _            _                 \n");
	printf("               / ___|| |_ ___| | | __ _ _ __(_)_   _ _ __ ___  \n");
	printf("               \\___ \\| __/ _ \\ | |/ _` | '__| | | | | '_ ` _ \\ \n");
	printf("                ___) | |_  __/ | | (_| | |  | | |_| | | | | | |\n");
	printf("               |____/ \\__\\___|_|_|\\__,_|_|  |_|\\__,_|_| |_| |_|\n\n");
	printf("       _____                          _      __  __       _             \n");
	printf("      |  ___|__  _ __ _ __ ___   __ _| |_   |  \\/  | __ _| | _____ _ __ \n");
	printf("      | |_ / _ \\| '__| '_ ` _ \\ / _` | __|  | |\\/| |/ _` | |/ / _ \\ '__|\n");
	printf("      |  _| (_) | |  | | | | | | (_| | |_   | |  | | (_| |   <  __/ |   \n");
	printf("      |_|  \\___/|_|  |_| |_| |_|\\__,_|\\__|  |_|  |_|\\__,_|_|\\_\\___|_|\n\n\n\n\n\n");
}

void screen_imp (){

	fflush(stdin);
	fflush(stdout);
	system("CLS");
	printf("\n");
	printf("  Importing %s (%s) format...\n\n", input_format, soft);
	printf(" =============================================================================\n");
	printf("     1.   Main Menu   |   2.   Exit   \n");
	printf(" =============================================================================\n\n");
}

void screen_exp1 (){

	fflush(stdin);
	fflush(stdout);
	system("CLS");
	printf("\n");
	printf("  Exporting %s format...\n\n", input_format);
	printf(" =============================================================================\n");
	printf("     1.   Main Menu   |   2.   Exit   \n");
	printf(" =============================================================================\n\n");
}

void screen_exp2 (){

	fflush(stdin);
	fflush(stdout);
	system("CLS");
	printf("\n");
	printf("  Exporting %s as %s format...\n\n", input_format, output_format);
	printf(" =============================================================================\n");
	printf("     1.   Main Menu   |   2.   Exit   \n");
	printf(" =============================================================================\n\n");
}

void screen_exp3 (){

	fflush(stdin);
	fflush(stdout);
	system("CLS");
	printf("\n");
	printf("  %s exported as %s format...\n\n", input_format, output_format);
	printf(" =============================================================================\n");
	printf("     1.   Main Menu   |   2.   Exit   \n");
	printf(" =============================================================================\n\n");
}


int import_menu (){

	int b;
	char a, c;
	FILE *fin;

	Ncmt=0;

	screen_imp();

	unos:
	printf("  Enter input filename: ");
	scanf("%s", fin_name);

	b=atoi(fin_name);
	if ((b==1 && (fin_name[1]=='\0')) ||
		(b==2 && (fin_name[1]=='\0'))) return b;

	fin=fopen(fin_name, "r");
	if (fin==NULL) {
		printf("\n  Error opening file %s\n\n", fin_name);
		goto unos;
	}

	else printf("\n  File %s is successfully opened\n", fin_name);

	while ((c=fgetc(fin)) != EOF){
		if (c=='\n') Ncmt++;
	}

	if(type==17) Ncmt=Ncmt/13;							// jer je 17. format cfw, a jedan komet je definiran kroz 13 redova
	if(type==3 || type==8 || type==10) Ncmt=Ncmt/2;		// kao gore, samo �to je 1 komet kroz 2 reda

	printf("\n  Total detected comets: %d\n  ", Ncmt);
	printf("\n  Press any key to continue... ");
	getch();

//ovaj do stavit pod if, i onda samo celestia ili i cel i stell, pa else
	do {
		screen_exp1();
		printf("  Export as:\n\n");
		printf("	    a. Celestia (SSC)\n");
		printf("	    b. Stellarium\n\n");
		printf("  Select option:   ");
		scanf("%c", &a);
	} while (a!='a' && a!='b' && a!='1' && a!='2');


	if (a=='1') return 1;
	if (a=='2') return 2;

	if (a=='a') output_format="Celestia (SSC)";
	if (a=='b') output_format="Stellarium";

	screen_exp2();
	printf("  Enter output filename: ");
	scanf("%s", fout_name);

	b=atoi(fout_name);
	if ((b==1 && (fout_name[1]=='\0')) ||
		(b==2 && (fout_name[1]=='\0'))) return b;

	if(type==0) import_mpc();
	if(type==1) import_skymap();
	if(type==2) import_guide();
	if(type==3) import_xephem();
	if(type==4) import_home_planet();
	if(type==5) import_mystars();
	if(type==6 || type==16) import_thesky();		//jer imaju isti format
	if(type==7) import_starry_night();
	if(type==8) import_deep_space();
	if(type==9) import_pc_tcs();
	if(type==10) import_ecu();
	if(type==11) import_dance();
	if(type==12) import_megastar();
	if(type==13) import_skychart();
	if(type==14) import_voyager();
	if(type==15) import_skytools();
	if(type==17) import_cfw();
	if(type==18) import_nasa1();
	if(type==19) import_nasa2();

	fclose(fin);

	if (a=='a') output_ssc ();
	if (a=='b') output_stell ();

	do {
		screen_exp3();
		printf("  Done\n\n  %d comets successfully saved in file %s\n\n", Ncmt, fout_name);
		printf("  Select option: ");
		scanf("%d", &b);
	} while (b!=1 && b!=2);

	return b;
}


void import_mpc(){

	int i;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		fscanf(fin, "%14c %d %d %d.%d %f %f %f %f %f %d %f %f %70[^\n]%*c",
			comet[i].x, &comet[i].y, &comet[i].m, &comet[i].d, &comet[i].h,
			&comet[i].q, &comet[i].e, &comet[i].pn, &comet[i].an,
			&comet[i].i, &comet[i].eq, &comet[i].H, &comet[i].G, comet[i].name);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].name);
	}
}

void import_skymap(){

	int i;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		fscanf(fin, "%47c %4d %2d %2d.%4d %f %f %f %f %f %f %10[^\n]%*c",
				comet[i].name, &comet[i].y, &comet[i].m, &comet[i].d,
				&comet[i].h, &comet[i].q, &comet[i].e, &comet[i].pn,
				&comet[i].an, &comet[i].i, &comet[i].H, &comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].name);
	}
}

void import_guide(){

	int i, j, k, l, t;
	char c;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {
		j=0;
		while ((c=fgetc(fin)) != '(' ){
			comet[i].name[j++]=c;
		}
		t=j;
		comet[i].name[t-1]='\0';

		if (comet[i].name[0]=='P' && comet[i].name[1]=='/')
			for (k=0; k<strlen(comet[i].name); k++)
				comet[i].name[k]=comet[i].name[k+2];		//k+2 jer je na mjestu 0='P', 1='/'

		l=0;
		while ((c=fgetc(fin)) != ')' ){
			comet[i].ID[l++]=c;
		}
		t=l;
		comet[i].ID[t]='\0';

		fscanf(fin, "%d.%d %d %d %10c %f %f %f %f %f %8c %f %f %15[^\n]%*c",
				&comet[i].d, &comet[i].h, &comet[i].m, &comet[i].y, comet[i].x,
				&comet[i].q, &comet[i].e, &comet[i].i, &comet[i].pn, &comet[i].an,
				comet[i].x, &comet[i].H, &comet[i].G, comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].ID);
	}
}

void import_xephem(){

	int JD, i, j, x;
	float a, n;
	char c;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		fscanf(fin, "%50[^\n]%*c", comet[i].x);

		j=0;
		while ((c=fgetc(fin)) != ',' ){
			comet[i].name[j++]=c;
		}

		c=fgetc(fin);

		if(c == 'e'){
			fscanf(fin, ",%f,%f,%f,%f,%f,%f,%d.%d,%d/%d.%d/%d,%d,g %f,%f\n",
				&comet[i].i, &comet[i].an, &comet[i].pn, &a,
				&n, &comet[i].e, &JD, &comet[i].h, &comet[i].m, &comet[i].d,
				&x, &comet[i].y, &comet[i].eq, &comet[i].H, &comet[i].G);

			comet[i].q = a*(1-comet[i].e);
			comet[i].JD = JD + compute_JD (comet[i].y, comet[i].m, comet[i].d);
		}

		else if(c == 'p'){
			fscanf(fin, ",%d/%d.%d/%d,%f,%f,%f,%f,%d,%f,%f\n",
				&comet[i].m, &comet[i].d, &comet[i].h, &comet[i].y,
				&comet[i].i, &comet[i].pn, &comet[i].q, &comet[i].an,
				&comet[i].eq, &comet[i].H, &comet[i].G);

			comet[i].e = 1.000000;
			comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		}

		else if(c == 'h'){
			fscanf(fin, ",%d/%d.%d/%d,%f,%f,%f,%f,%f,%d,%f,%f\n",
				&comet[i].m, &comet[i].d, &comet[i].h, &comet[i].y,
				&comet[i].i, &comet[i].an, &comet[i].pn, &comet[i].e,
				&comet[i].q, &comet[i].eq, &comet[i].H, &comet[i].G);

			comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		}

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		edit_name (comet[i].name);
	}
}

void import_voyager(){

	int i;
	char mj[3+1];
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		fscanf(fin, "%28c %f %f %f %f %f %f %4d %3c %d.%d %15[^\n]%*c",
				comet[i].name, &comet[i].q, &comet[i].e, &comet[i].i,
				&comet[i].an, &comet[i].pn, &comet[i].G, &comet[i].y,
				mj, &comet[i].d, &comet[i].h, comet[i].x);

		if (mj[0]=='J' && mj[1]=='a' && mj[2]=='n') comet[i].m=1;
		if (mj[0]=='F' && mj[1]=='e' && mj[2]=='b') comet[i].m=2;
		if (mj[0]=='M' && mj[1]=='a' && mj[2]=='r') comet[i].m=3;
		if (mj[0]=='A' && mj[1]=='p' && mj[2]=='r') comet[i].m=4;
		if (mj[0]=='M' && mj[1]=='a' && mj[2]=='y') comet[i].m=5;
		if (mj[0]=='J' && mj[1]=='u' && mj[2]=='n') comet[i].m=6;
		if (mj[0]=='J' && mj[1]=='u' && mj[2]=='l') comet[i].m=7;
		if (mj[0]=='A' && mj[1]=='u' && mj[2]=='g') comet[i].m=8;
		if (mj[0]=='S' && mj[1]=='e' && mj[2]=='p') comet[i].m=9;
		if (mj[0]=='O' && mj[1]=='c' && mj[2]=='t') comet[i].m=10;
		if (mj[0]=='N' && mj[1]=='o' && mj[2]=='v') comet[i].m=11;
		if (mj[0]=='D' && mj[1]=='e' && mj[2]=='c') comet[i].m=12;

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].name);
	}
}

void import_home_planet(){

	int i, j;
	char c;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		j=0;
		while ((c=fgetc(fin)) != ',' ){
			comet[i].name[j++]=c;
		}

		fscanf(fin, "%d-%d-%d.%d,%f,%f,%f,%f,%f %50[^\n]%*c",
			&comet[i].y, &comet[i].m, &comet[i].d, &comet[i].h,
			&comet[i].q, &comet[i].e, &comet[i].pn, &comet[i].an,
			&comet[i].i, comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].name);
	}
}

void import_mystars(){

	int i, j;
	char c;
	FILE *fin;

// 	varijable za izracun gregorijanskog datuma iz julijanskog dana
	int v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		j=0;
		while ((c=fgetc(fin)) != ';' ){
			comet[i].name[j++]=c;
		}

		fscanf(fin, "%d.%d %f %f %f %f %f %f %f %30[^\n]%*c",
				&comet[i].JD, &comet[i].h, &comet[i].pn, &comet[i].e,
				&comet[i].q, &comet[i].i, &comet[i].an, &comet[i].H,
				&comet[i].G, comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = comet[i].JD + 2400000;
		edit_name (comet[i].name);

//		izracuvanje gregorijanskog datuma iz julijanskog dana
//		izvor: http://en.wikipedia.org/wiki/Julian_day#Gregorian_calendar_from_Julian_day_number

		v1 = comet[i].JD + 0.5;
		v2 = v1 + 32044;
		v3 = v2 / 146097;
		v4 = v2 % 146097;
        v5 = (v4 / 36524 + 1) * 3 / 4;
        v6 = v4 - v5 * 36524;
        v7 = v6 / 1461;
        v8 = v6 % 1461;
        v9 = (v8 / 365 + 1) * 3 / 4;
        v10 = v8 - v9 * 365;
        v12 = v3 * 400 + v5 * 100 + v7 * 4 + v9;
        v11 = (v10 * 5 + 308) / 153 - 2;
        v13 = v10 - (v11 + 4) * 153 / 5 + 122;
        comet[i].y = v12 - 4800 + (v11 + 2) / 12;
		comet[i].m = (v11 + 2) % 12 + 1;
		comet[i].d = v13 + 2;
	}
}

void import_thesky(){

	int i;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {
//		fscanf(fin, "%40c %*c %d %*c %4d %2d %2d %*c %d %*c %f %*c %f %*c %f %*c %f %*c %f %*c %f %25[^\n]%*c",     stari nacin
		fscanf(fin, "%45c %4d%2d%2d.%d | %f | %f | %f | %f | %f | %f %25[^\n]%*c",
				comet[i].name, &comet[i].y, &comet[i].m,
				&comet[i].d, &comet[i].h, &comet[i].q, &comet[i].e,
				&comet[i].pn, &comet[i].an, &comet[i].i, &comet[i].H, comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].name);
	}
}

void import_starry_night(){

	int i;
	FILE *fin;

// 	varijable za izracun gregorijanskog datuma iz julijanskog dana
	int v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12, v13;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		fscanf(fin, "%5c %30c %f %f %f %f %f %f %f %d.%d %14c %f %16c %15[^\n]%*c",
				comet[i].x, comet[i].name, &comet[i].H, &comet[i].G, &comet[i].e, &comet[i].q,
				&comet[i].an, &comet[i].pn, &comet[i].i, &comet[i].JD, &comet[i].h,
				comet[i].x, &comet[i].G, comet[i].ID, comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		edit_name (comet[i].name);
		edit_name (comet[i].ID);

//		izracuvanje gregorijanskog datuma iz julijanskog dana
//		izvor: http://en.wikipedia.org/wiki/Julian_day#Gregorian_calendar_from_Julian_day_number

		v1 = comet[i].JD + 0.5;
		v2 = v1 + 32044;
		v3 = v2 / 146097;
		v4 = v2 % 146097;
        v5 = (v4 / 36524 + 1) * 3 / 4;
        v6 = v4 - v5 * 36524;
        v7 = v6 / 1461;
        v8 = v6 % 1461;
        v9 = (v8 / 365 + 1) * 3 / 4;
        v10 = v8 - v9 * 365;
        v12 = v3 * 400 + v5 * 100 + v7 * 4 + v9;
        v11 = (v10 * 5 + 308) / 153 - 2;
        v13 = v10 - (v11 + 4) * 153 / 5 + 122;
        comet[i].y = v12 - 4800 + (v11 + 2) / 12;
		comet[i].m = (v11 + 2) % 12 + 1;
		comet[i].d = v13 + 2;
	}
}

void import_deep_space(){

	int i, j, t;
	char c;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		j=0;
		while ((c=fgetc(fin)) != '(' ){
			comet[i].name[j++]=c;
		}
		t=j;
		comet[i].name[t-1]='\0';

		j=0;
		while ((c=fgetc(fin)) != ')' ){
			comet[i].ID[j++]=c;
		}
		t=j;
		comet[i].ID[t]='\0';

		fscanf(fin, "\n%8c %d %d %d.%d %f %f %f %f %f %f %f\n",
			comet[i].x, &comet[i].y, &comet[i].m, &comet[i].d, &comet[i].h,
			&comet[i].q, &comet[i].e, &comet[i].pn, &comet[i].an,
			&comet[i].i, &comet[i].H, &comet[i].G);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].ID);
	}
}

void import_pc_tcs(){

	int i;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		fscanf(fin, "%s %f %f %f %f %f %d %d %d.%d %f %f %60[^\n]%*c",
				comet[i].ID, &comet[i].q, &comet[i].e, &comet[i].i,
				&comet[i].pn, &comet[i].an, &comet[i].y, &comet[i].m,
				&comet[i].d, &comet[i].h, &comet[i].H, &comet[i].G, comet[i].name);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].ID);
		edit_name (comet[i].name);
	}
}

void import_skytools(){

	int i;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		fscanf(fin, "%2c %52c %d %d %d.%d %f %f %f %f %f %f %30[^\n]%*c",
				comet[i].x, comet[i].name, &comet[i].y, &comet[i].m, &comet[i].d,
				&comet[i].h, &comet[i].q, &comet[i].e, &comet[i].pn,
				&comet[i].an, &comet[i].i, &comet[i].H, comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].name);
	}
}

void import_skychart(){

	int i, j;
	char c;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		fscanf(fin, "%12c %f %f %f %f %f %d %d/%d/%d.%d %f %f %d %d %75[^\n]%*c",
				comet[i].x, &comet[i].q, &comet[i].e, &comet[i].i, &comet[i].pn,
				&comet[i].an, &comet[i].eq, &comet[i].y, &comet[i].m, &comet[i].d,
				&comet[i].h, &comet[i].H, &comet[i].G, &comet[i].eq, &comet[i].eq,
				comet[i].name);

		for(j=0; j<strlen(comet[i].name); j++)
			if(comet[i].name[j]==';') comet[i].name[j]='\0';

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].name);
	}
}

void import_ecu(){

	int i;
	char c;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		fscanf(fin, "%45[^\n]%*c %8c %d %d %d.%d %f %f %f %f %f %f %10[^\n]%*c",
			comet[i].name, comet[i].x, &comet[i].y, &comet[i].m, &comet[i].d,
			&comet[i].h, &comet[i].q, &comet[i].e, &comet[i].pn, &comet[i].an,
			&comet[i].i, &comet[i].H, &comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].name);
	}
}

void import_dance(){

	int i;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		fscanf(fin, "%11c %f %f %f %f %f %d.%2d%2d%4d %30[^\n]%*c",
				comet[i].ID, &comet[i].q, &comet[i].e, &comet[i].i,
				&comet[i].an, &comet[i].pn, &comet[i].y, &comet[i].m,
				&comet[i].d, &comet[i].h, comet[i].name);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].ID);
		edit_name (comet[i].name);
	}
}

void import_megastar(){

	int i;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		fscanf(fin, "%30c %12c %d %d %d.%d %f %f %f %f %f %f %f %25[^\n]%*c",
				comet[i].name, comet[i].ID, &comet[i].y, &comet[i].m, &comet[i].d,
				&comet[i].h, &comet[i].q, &comet[i].e, &comet[i].pn,
				&comet[i].an, &comet[i].i, &comet[i].H, &comet[i].G, comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].ID);
		edit_name (comet[i].name);
	}
}

void import_cfw(){

	int i;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {
		fscanf(fin, "name=%40[^\n]%*c\
					%20[^\n]%*c\
					type=orbit\n\
					T=%d %d %d.%d\n\
					q=%f\n\
					e=%f\n\
					peri=%f\n\
					node=%f\n\
					i=%f\n\
					prec=2000.0\n\
					%20[^\n]%*c\
					mageq=%f %10[^\n]%*c\
					\n",
					comet[i].name,
					comet[i].x,
					&comet[i].y, &comet[i].m, &comet[i].d, &comet[i].h,
					&comet[i].q,
					&comet[i].e,
					&comet[i].pn,
					&comet[i].an,
					&comet[i].i,
					comet[i].x,
					&comet[i].H,
					comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name (comet[i].name);
	}
}

void import_nasa1(){

	int i, j, k;
	char c;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		k=0;
		j=0;
		while (k<39){
			c=fgetc(fin);
			comet[i].name[j]=c;
			if (k<5 && comet[i].name[j]==' ') --j;
			j++;
			k++;
		}

		fscanf(fin, "%d %f %f %f %f %f %4d%2d%2d.%4d %15[^\n]%*c",
				&comet[i].eq, &comet[i].q, &comet[i].e,
				&comet[i].i, &comet[i].pn, &comet[i].an, &comet[i].y,
				&comet[i].m, &comet[i].d, &comet[i].h, comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name(comet[i].name);
	}
}

void import_nasa2(){

	int i, j;
	char c;
	FILE *fin;

	fin=fopen(fin_name, "r");

	for (i=0; i<Ncmt; i++) {

		j=0;
		c=fgetc(fin);		// da uzme prve navodnike
		while ((c=fgetc(fin)) != '"' ){
			comet[i].name[j]=c;
			if (c==' ' && j<1) --j;
			j++;
		}

		comet[i].name[j]='\0';

		fscanf(fin, ",%f,%f,%f,%f,%f,%4d%2d%2d.%4d%10[^\n]%*c",
				&comet[i].q, &comet[i].e, &comet[i].pn, &comet[i].an,
				&comet[i].i, &comet[i].y, &comet[i].m,
				&comet[i].d, &comet[i].h, comet[i].x);

		comet[i].P = compute_period (comet[i].q, comet[i].e);
		comet[i].JD = compute_JD (comet[i].y, comet[i].m, comet[i].d);
		edit_name(comet[i].name);
	}
}

float compute_period (float q, float e){

	float P;

	if (e >= 0 && e < 1)
		P = pow((q/(1-e)),1.5);

	if (e > 1)
		P = pow((q/(e-1)),1.5);

	if (e == 1)
		P = pow((q/(1-0.999999)),1.5);

	return P;
}

int compute_JD (int y, int m, int d){

	int JD;

	JD = 367*y - (7*(y + (m + 9)/12))/4 -
		((3*(y + (m - 9)/7))/100 + 1)/4 +
		(275*m)/9 + d + 1721029;

	return JD;
}

char *edit_name (char *name){

	int i, j, k;

	for (j=0; name[j+1]!='\0'; j++) {
		if (name[j]==' ' && name[j+1]==' ') {
			name[j]='\0';
			break;
		}
		if (name[j]=='/')
			name[j]=' ';
	}
}


void output_ssc (){

	int i;
	char *mon;
	FILE *fout;

	for (i=0; i<Ncmt; i++) {

		if (comet[i].m==1) mon="Jan";
		if (comet[i].m==2) mon="Feb";
		if (comet[i].m==3) mon="Mar";
		if (comet[i].m==4) mon="Apr";
		if (comet[i].m==5) mon="May";
		if (comet[i].m==6) mon="Jun";
		if (comet[i].m==7) mon="Jul";
		if (comet[i].m==8) mon="Aug";
		if (comet[i].m==9) mon="Sep";
		if (comet[i].m==10) mon="Oct";
		if (comet[i].m==11) mon="Nov";
		if (comet[i].m==12) mon="Dec";

		fout=fopen(fout_name, "a");

	if (type==2 || type==7 || type==8 || type==9 || type==11 || type==12)
		fprintf(fout,"\"%s %s\" \"Sol\"\n", comet[i].ID, comet[i].name);
	else
		fprintf(fout,"\"%s\" \"Sol\"\n", comet[i].name);
		fprintf(fout,"{\n");
		fprintf(fout,"Class \"comet\" \n");
		fprintf(fout,"Mesh \"asteroid.cms\" \n");
		fprintf(fout,"Texture \"asteroid.jpg\" \n");
		fprintf(fout,"Radius 5 \n");
		fprintf(fout,"Albedo 0.1 \n");
		fprintf(fout,"EllipticalOrbit \n");
		fprintf(fout,"\t{ \n");
		fprintf(fout,"\tPeriod \t\t\t %f \n", comet[i].P);
		fprintf(fout,"\tPericenterDistance \t %f \n", comet[i].q);
		fprintf(fout,"\tEccentricity \t\t %f \n", comet[i].e);
		fprintf(fout,"\tInclination \t\t %.4f \n", comet[i].i);
		fprintf(fout,"\tAscendingNode \t\t %.4f \n", comet[i].an);
		fprintf(fout,"\tArgOfPericenter \t %.4f \n", comet[i].pn);
		fprintf(fout,"\tMeanAnomaly \t\t 0  \n");
	if (type==15)
		fprintf(fout,"\tEpoch \t\t\t %d.%.d\t# %d %s %.2d.%.d \n", comet[i].JD, comet[i].h, comet[i].y, mon, comet[i].d, comet[i].h);
	else
		fprintf(fout,"\tEpoch \t\t\t %d.%.4d\t# %d %s %.2d.%.4d \n", comet[i].JD, comet[i].h, comet[i].y, mon, comet[i].d, comet[i].h);
		fprintf(fout,"\t} \n");
		fprintf(fout,"}\n\n\n");

		fclose(fout);
	}
}

void output_stell (){

	int i;
	FILE *fout;

	for (i=0; i<Ncmt; i++) {

		fout=fopen(fout_name, "a");

	if (type==2 || type==7 || type==8 || type==9 || type==11 || type==12)
		fprintf(fout,"[%s %s]\n", comet[i].ID, comet[i].name);
	else
		fprintf(fout,"[%s]\n", comet[i].name);
		fprintf(fout,"parent=Sun\n");
		fprintf(fout,"orbit_Inclination=%f\n", comet[i].i);
		fprintf(fout,"coord_func=comet_orbit\n");
		fprintf(fout,"orbit_Eccentricity=%f\n", comet[i].e);
		fprintf(fout,"orbit_ArgOfPericenter=%f\n", comet[i].pn);
		fprintf(fout,"absolute_magnitude=%.1f\n", comet[i].H);
		fprintf(fout,"name=%s\n", comet[i].name);
		fprintf(fout,"slope_parameter=4\n");
		fprintf(fout,"lighting=false\n");
		fprintf(fout,"tex_map=nomap.png\n");
		fprintf(fout,"color=1.0, 1.0, 1.0\n");
		fprintf(fout,"orbit_AscendingNode=%f\n", comet[i].an);
		fprintf(fout,"albedo=1\n");
		fprintf(fout,"radius=5\n");
		fprintf(fout,"orbit_PericenterDistance=%f\n", comet[i].q);
		fprintf(fout,"type=comet\n");
	if (type==15)
		fprintf(fout,"orbit_TimeAtPericenter=%d.%.d\n\n", comet[i].JD, comet[i].h);
	else
		fprintf(fout,"orbit_TimeAtPericenter=%d.%.4d\n\n", comet[i].JD, comet[i].h);

		fclose(fout);
	}
}
