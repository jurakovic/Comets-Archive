//---------------------------------------------------------------------------

#ifndef Unit12H
#define Unit12H
//---------------------------------------------------------------------------
#include <System.Classes.hpp>
#include <Vcl.Controls.hpp>
#include <Vcl.StdCtrls.hpp>
#include <Vcl.Forms.hpp>
#include "CometMain.hpp"

//---------------------------------------------------------------------------
class TForm12 : public TForm
{
__published:	// IDE-managed Components
	TEdit *Edit1;
	TEdit *Edit10;
	TEdit *Edit11;
	TEdit *Edit12;
	TEdit *Edit13;
	TEdit *Edit14;
	TEdit *Edit15;
	TEdit *Edit2;
	TEdit *Edit3;
	TEdit *Edit4;
	TEdit *Edit5;
	TEdit *Edit6;
	TEdit *Edit7;
	TEdit *Edit8;
	TEdit *Edit9;
	TLabel *Label10;
	TLabel *Label11;
	TLabel *Label12;
	TLabel *Label13;
	TLabel *Label14;
	TLabel *Label15;
	TLabel *Label16;
	TLabel *Label17;
	TLabel *Label18;
	TLabel *Label19;
	TLabel *Label2;
	TLabel *Label3;
	TLabel *Label4;
	TLabel *Label5;
	TLabel *Label6;
	TLabel *Label7;
	TLabel *Label8;
	TLabel *Label9;
	void __fastcall FormShow(TObject *Sender);
	void __fastcall FormClose(TObject *Sender, TCloseAction &Action);
private:	// User declarations
public:		// User declarations
	__fastcall TForm12(TComponent* Owner);
	void updateEditFields(Comet *);
};
//---------------------------------------------------------------------------
extern PACKAGE TForm12 *Form12;
//---------------------------------------------------------------------------
#endif
