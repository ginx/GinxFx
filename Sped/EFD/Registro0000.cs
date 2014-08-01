using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sped.EFD
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexAttribute : Attribute
    {
        public int Index { get; private set; }

        public IndexAttribute (int index)
	    {
            this.Index = index;
	    }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FormatAttribute : Attribute
    {
        public string Format { get; private set; }

        public FormatAttribute(string format)
        {
            this.Format = format;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RegexValidatorAttribute : Attribute
    {
        public string Regex { get; private set; }

        public RegexValidatorAttribute(string regex)
        {
            this.Regex = regex;
        }
    }

    public abstract class Registro
    {
        [Index(1)]
        public abstract string REG { get; }
    }

    public enum TipoEscrituracao
    {
        [Description("Original")]
        Original = 0,

        [Description("Retificadora")]
        Retificadora = 1
    }

    public enum IndicadorSituacaoEspecial
    {
        [Description("Abertura")]
        Abertura = 0,

        [Description("Cisão")]
        Cisao = 1,

        [Description("Fusão")]
        Fusao = 2,

        [Description("Incorporação")]
        Incorporacao = 3,

        [Description("Encerramento")]
        Encerramento = 4
    }

    public enum IndicadorAtividade
    {
        [Description("Industrial ou equiparado a industrial")]
        IndustriaOuEquiparado = 0,

        [Description("Prestador de serviços")]
        PrestadorServico = 1,

        [Description("Atividade de comércio")]
        Comercio = 2,

        [Description("Atividade financeira")]
        Financeira = 3,

        [Description("Atividade imobiliária")]
        Imobiliaria = 4,

        [Description("Outros")]
        Outros = 9
    }

    public enum IndicadorMovimento
    {
        [Description("Bloco com dados informados")]
        BlocoComDados = 0,

        [Description("Bloco sem dados informados")]
        BlocoSemDados = 1
    }

    public enum IndicadorNaturezaPessoaJuridica
    {
        [Description("Pessoa jurídica em geral (não participante de SCP como sócia ostensiva)")]
        PessoaJuridicaEmGeralNaoSCP = 0,

        [Description("Sociedade cooperativa (não participante de SCP como sócia ostensiva)")]
        SociedadeCooperativaNaoSCP = 1,

        [Description("Entidade sujeita ao PIS/Pasep exclusivamente com base na Folha de Salários")]
        EntidadeSujeitaAoPISPasepBaseadoNaFolha = 2,

        [Description("Pessoa jurídica em geral participante de SCP como sócia ostensiva")]
        PessoaJuridicaEmGeralParticipanteSCP = 3,

        [Description("Sociedade cooperativa  participante de SCP como sócia ostensiva")]
        SociedadeCooperativaParticipanteSCP = 4,

        [Description("Sociedade em Conta de Participação - SCP")]
        SociedadeContaParticipacao = 5,
    }

    public class Registro0000 : Registro 
    {
        public override string REG 
        {
            get { return "0000"; } 
        }

        [Index(2)]
        [Format("000")]
        public int COD_VER
        {
            get { return 3; }
        }

        [Index(3)]
        [Format("0")]
        public TipoEscrituracao TIPO_ESCRIT
        {
            get;
            set;
        }

        [Index(4)]
        [Format("0")]
        public IndicadorSituacaoEspecial IND_SIT_ESP { get; set; }

        [Index(5)]
        // Lenght 41
        public string NUM_REC_ANTERIOR { get; set; }

        [Index(6)]
        public DateTime DT_INI { get; set; }

        [Index(7)]
        public DateTime DT_FIN { get; set; }

        [Index(8)]
        // Lenght 0-100
        public string NOME { get; set; }

        [Index(9)]
        [Format("00000000000000")]
        public long CNPJ { get; set; }

        [Index(10)]
        [RegexValidator("[A-Za-z]{2}")]
        public string UF { get; set; }

        [Index(11)]
        [Format("0000000")]
        public int COD_MUN { get; set; }

        [Index(12)]
        // Lenght 9
        public string SUFRAMA { get; set; }

        [Index(13)]
        [Format("00")]
        public IndicadorNaturezaPessoaJuridica IND_NAT_PJ { get; set; }

        [Index(14)]
        [Format("0")]
        public IndicadorAtividade IND_ATIV { get; set; }
    }

    public class Registro0001 : Registro 
    {
        [Index(1)]
        public override string REG 
        {
            get { return "0001"; } 
        }

        [Index(2)]
        public IndicadorMovimento IND_MOV
        {
            get;
            set;
        }

        public ICollection<Registro0035> Registros0035 { get; set; }
        public ICollection<Registro0100> Registros0100 { get; set; }
        public ICollection<Registro0110> Registros0110 { get; set; }
    }

    public class Registro0035 : Registro { public override string REG { get { return "0035"; } } }
    public class Registro0100 : Registro { public override string REG { get { return "0100"; } } }
    public class Registro0110 : Registro { public override string REG { get { return "0110"; } } }
    public class Registro0111 : Registro { public override string REG { get { return "0111"; } } }
    public class Registro0120 : Registro { public override string REG { get { return "0120"; } } }
    public class Registro0140 : Registro { public override string REG { get { return "0140"; } } }
    public class Registro0145 : Registro { public override string REG { get { return "0145"; } } }
    public class Registro0150 : Registro { public override string REG { get { return "0150"; } } }
    public class Registro0190 : Registro { public override string REG { get { return "0190"; } } }
    public class Registro0200 : Registro { public override string REG { get { return "0200"; } } }
    public class Registro0205 : Registro { public override string REG { get { return "0205"; } } }
    public class Registro0206 : Registro { public override string REG { get { return "0206"; } } }
    public class Registro0208 : Registro { public override string REG { get { return "0208"; } } }
    public class Registro0400 : Registro { public override string REG { get { return "0400"; } } }
    public class Registro0450 : Registro { public override string REG { get { return "0450"; } } }
    public class Registro0500 : Registro { public override string REG { get { return "0500"; } } }
    public class Registro0600 : Registro { public override string REG { get { return "0600"; } } }
    public class Registro0990 : Registro { public override string REG { get { return "0990"; } } }
    public class RegistroA001 : Registro { public override string REG { get { return "A001"; } } }
    public class RegistroA010 : Registro { public override string REG { get { return "A010"; } } }
    public class RegistroA100 : Registro { public override string REG { get { return "A100"; } } }
    public class RegistroA110 : Registro { public override string REG { get { return "A110"; } } }
    public class RegistroA111 : Registro { public override string REG { get { return "A111"; } } }
    public class RegistroA120 : Registro { public override string REG { get { return "A120"; } } }
    public class RegistroA170 : Registro { public override string REG { get { return "A170"; } } }
    public class RegistroA990 : Registro { public override string REG { get { return "A990"; } } }
    public class RegistroC001 : Registro { public override string REG { get { return "C001"; } } }
    public class RegistroC010 : Registro { public override string REG { get { return "C010"; } } }
    public class RegistroC100 : Registro { public override string REG { get { return "C100"; } } }
    public class RegistroC110 : Registro { public override string REG { get { return "C110"; } } }
    public class RegistroC111 : Registro { public override string REG { get { return "C111"; } } }
    public class RegistroC120 : Registro { public override string REG { get { return "C120"; } } }
    public class RegistroC170 : Registro { public override string REG { get { return "C170"; } } }
    public class RegistroC175 : Registro { public override string REG { get { return "C175"; } } }
    public class RegistroC180 : Registro { public override string REG { get { return "C180"; } } }
    public class RegistroC181 : Registro { public override string REG { get { return "C181"; } } }
    public class RegistroC185 : Registro { public override string REG { get { return "C185"; } } }
    public class RegistroC188 : Registro { public override string REG { get { return "C188"; } } }
    public class RegistroC190 : Registro { public override string REG { get { return "C190"; } } }
    public class RegistroC191 : Registro { public override string REG { get { return "C191"; } } }
    public class RegistroC195 : Registro { public override string REG { get { return "C195"; } } }
    public class RegistroC198 : Registro { public override string REG { get { return "C198"; } } }
    public class RegistroC199 : Registro { public override string REG { get { return "C199"; } } }
    public class RegistroC380 : Registro { public override string REG { get { return "C380"; } } }
    public class RegistroC381 : Registro { public override string REG { get { return "C381"; } } }
    public class RegistroC385 : Registro { public override string REG { get { return "C385"; } } }
    public class RegistroC395 : Registro { public override string REG { get { return "C395"; } } }
    public class RegistroC396 : Registro { public override string REG { get { return "C396"; } } }
    public class RegistroC400 : Registro { public override string REG { get { return "C400"; } } }
    public class RegistroC405 : Registro { public override string REG { get { return "C405"; } } }
    public class RegistroC481 : Registro { public override string REG { get { return "C481"; } } }
    public class RegistroC485 : Registro { public override string REG { get { return "C485"; } } }
    public class RegistroC489 : Registro { public override string REG { get { return "C489"; } } }
    public class RegistroC490 : Registro { public override string REG { get { return "C490"; } } }
    public class RegistroC491 : Registro { public override string REG { get { return "C491"; } } }
    public class RegistroC495 : Registro { public override string REG { get { return "C495"; } } }
    public class RegistroC499 : Registro { public override string REG { get { return "C499"; } } }
    public class RegistroC500 : Registro { public override string REG { get { return "C500"; } } }
    public class RegistroC501 : Registro { public override string REG { get { return "C501"; } } }
    public class RegistroC505 : Registro { public override string REG { get { return "C505"; } } }
    public class RegistroC509 : Registro { public override string REG { get { return "C509"; } } }
    public class RegistroC600 : Registro { public override string REG { get { return "C600"; } } }
    public class RegistroC601 : Registro { public override string REG { get { return "C601"; } } }
    public class RegistroC605 : Registro { public override string REG { get { return "C605"; } } }
    public class RegistroC609 : Registro { public override string REG { get { return "C609"; } } }
    public class RegistroC800 : Registro { public override string REG { get { return "C800"; } } }
    public class RegistroC810 : Registro { public override string REG { get { return "C810"; } } }
    public class RegistroC820 : Registro { public override string REG { get { return "C820"; } } }
    public class RegistroC830 : Registro { public override string REG { get { return "C830"; } } }
    public class RegistroC850 : Registro { public override string REG { get { return "C850"; } } }
    public class RegistroC860 : Registro { public override string REG { get { return "C860"; } } }
    public class RegistroC870 : Registro { public override string REG { get { return "C870"; } } }
    public class RegistroC880 : Registro { public override string REG { get { return "C880"; } } }
    public class RegistroC890 : Registro { public override string REG { get { return "C890"; } } }
    public class RegistroC990 : Registro { public override string REG { get { return "C990"; } } }
    public class RegistroD001 : Registro { public override string REG { get { return "D001"; } } }
    public class RegistroD010 : Registro { public override string REG { get { return "D010"; } } }
    public class RegistroD100 : Registro { public override string REG { get { return "D100"; } } }
    public class RegistroD101 : Registro { public override string REG { get { return "D101"; } } }
    public class RegistroD105 : Registro { public override string REG { get { return "D105"; } } }
    public class RegistroD111 : Registro { public override string REG { get { return "D111"; } } }
    public class RegistroD200 : Registro { public override string REG { get { return "D200"; } } }
    public class RegistroD201 : Registro { public override string REG { get { return "D201"; } } }
    public class RegistroD205 : Registro { public override string REG { get { return "D205"; } } }
    public class RegistroD209 : Registro { public override string REG { get { return "D209"; } } }
    public class RegistroD300 : Registro { public override string REG { get { return "D300"; } } }
    public class RegistroD309 : Registro { public override string REG { get { return "D309"; } } }
    public class RegistroD350 : Registro { public override string REG { get { return "D350"; } } }
    public class RegistroD359 : Registro { public override string REG { get { return "D359"; } } }
    public class RegistroD500 : Registro { public override string REG { get { return "D500"; } } }
    public class RegistroD501 : Registro { public override string REG { get { return "D501"; } } }
    public class RegistroD505 : Registro { public override string REG { get { return "D505"; } } }
    public class RegistroD509 : Registro { public override string REG { get { return "D509"; } } }
    public class RegistroD600 : Registro { public override string REG { get { return "D600"; } } }
    public class RegistroD601 : Registro { public override string REG { get { return "D601"; } } }
    public class RegistroD605 : Registro { public override string REG { get { return "D605"; } } }
    public class RegistroD609 : Registro { public override string REG { get { return "D609"; } } }
    public class RegistroD990 : Registro { public override string REG { get { return "D990"; } } }
    public class RegistroF001 : Registro { public override string REG { get { return "F001"; } } }
    public class RegistroF010 : Registro { public override string REG { get { return "F010"; } } }
    public class RegistroF100 : Registro { public override string REG { get { return "F100"; } } }
    public class RegistroF111 : Registro { public override string REG { get { return "F111"; } } }
    public class RegistroF120 : Registro { public override string REG { get { return "F120"; } } }
    public class RegistroF129 : Registro { public override string REG { get { return "F129"; } } }
    public class RegistroF130 : Registro { public override string REG { get { return "F130"; } } }
    public class RegistroF139 : Registro { public override string REG { get { return "F139"; } } }
    public class RegistroF150 : Registro { public override string REG { get { return "F150"; } } }
    public class RegistroF200 : Registro { public override string REG { get { return "F200"; } } }
    public class RegistroF205 : Registro { public override string REG { get { return "F205"; } } }
    public class RegistroF210 : Registro { public override string REG { get { return "F210"; } } }
    public class RegistroF211 : Registro { public override string REG { get { return "F211"; } } }
    public class RegistroF500 : Registro { public override string REG { get { return "F500"; } } }
    public class RegistroF509 : Registro { public override string REG { get { return "F509"; } } }
    public class RegistroF510 : Registro { public override string REG { get { return "F510"; } } }
    public class RegistroF519 : Registro { public override string REG { get { return "F519"; } } }
    public class RegistroF525 : Registro { public override string REG { get { return "F525"; } } }
    public class RegistroF550 : Registro { public override string REG { get { return "F550"; } } }
    public class RegistroF559 : Registro { public override string REG { get { return "F559"; } } }
    public class RegistroF560 : Registro { public override string REG { get { return "F560"; } } }
    public class RegistroF569 : Registro { public override string REG { get { return "F569"; } } }
    public class RegistroF600 : Registro { public override string REG { get { return "F600"; } } }
    public class RegistroF700 : Registro { public override string REG { get { return "F700"; } } }
    public class RegistroF800 : Registro { public override string REG { get { return "F800"; } } }
    public class RegistroF990 : Registro { public override string REG { get { return "F990"; } } }
    public class RegistroI001 : Registro { public override string REG { get { return "I001"; } } }
    public class RegistroI010 : Registro { public override string REG { get { return "I010"; } } }
    public class RegistroI100 : Registro { public override string REG { get { return "I100"; } } }
    public class RegistroI199 : Registro { public override string REG { get { return "I199"; } } }
    public class RegistroI200 : Registro { public override string REG { get { return "I200"; } } }
    public class RegistroI299 : Registro { public override string REG { get { return "I299"; } } }
    public class RegistroI300 : Registro { public override string REG { get { return "I300"; } } }
    public class RegistroI399 : Registro { public override string REG { get { return "I399"; } } }
    public class RegistroI990 : Registro { public override string REG { get { return "I990"; } } }
    public class RegistroM001 : Registro { public override string REG { get { return "M001"; } } }
    public class RegistroM100 : Registro { public override string REG { get { return "M100"; } } }
    public class RegistroM105 : Registro { public override string REG { get { return "M105"; } } }
    public class RegistroM110 : Registro { public override string REG { get { return "M110"; } } }
    public class RegistroM115 : Registro { public override string REG { get { return "M115"; } } }
    public class RegistroM200 : Registro { public override string REG { get { return "M200"; } } }
    public class RegistroM205 : Registro { public override string REG { get { return "M205"; } } }
    public class RegistroM210 : Registro { public override string REG { get { return "M210"; } } }
    public class RegistroM211 : Registro { public override string REG { get { return "M211"; } } }
    public class RegistroM220 : Registro { public override string REG { get { return "M220"; } } }
    public class RegistroM225 : Registro { public override string REG { get { return "M225"; } } }
    public class RegistroM230 : Registro { public override string REG { get { return "M230"; } } }
    public class RegistroM300 : Registro { public override string REG { get { return "M300"; } } }
    public class RegistroM350 : Registro { public override string REG { get { return "M350"; } } }
    public class RegistroM400 : Registro { public override string REG { get { return "M400"; } } }
    public class RegistroM410 : Registro { public override string REG { get { return "M410"; } } }
    public class RegistroM500 : Registro { public override string REG { get { return "M500"; } } }
    public class RegistroM505 : Registro { public override string REG { get { return "M505"; } } }
    public class RegistroM510 : Registro { public override string REG { get { return "M510"; } } }
    public class RegistroM515 : Registro { public override string REG { get { return "M515"; } } }
    public class RegistroM600 : Registro { public override string REG { get { return "M600"; } } }
    public class RegistroM605 : Registro { public override string REG { get { return "M605"; } } }
    public class RegistroM610 : Registro { public override string REG { get { return "M610"; } } }
    public class RegistroM611 : Registro { public override string REG { get { return "M611"; } } }
    public class RegistroM620 : Registro { public override string REG { get { return "M620"; } } }
    public class RegistroM625 : Registro { public override string REG { get { return "M625"; } } }
    public class RegistroM630 : Registro { public override string REG { get { return "M630"; } } }
    public class RegistroM700 : Registro { public override string REG { get { return "M700"; } } }
    public class RegistroM800 : Registro { public override string REG { get { return "M800"; } } }
    public class RegistroM810 : Registro { public override string REG { get { return "M810"; } } }
    public class RegistroM990 : Registro { public override string REG { get { return "M990"; } } }
    public class RegistroP001 : Registro { public override string REG { get { return "P001"; } } }
    public class RegistroP010 : Registro { public override string REG { get { return "P010"; } } }
    public class RegistroP100 : Registro { public override string REG { get { return "P100"; } } }
    public class RegistroP110 : Registro { public override string REG { get { return "P110"; } } }
    public class RegistroP199 : Registro { public override string REG { get { return "P199"; } } }
    public class RegistroP200 : Registro { public override string REG { get { return "P200"; } } }
    public class RegistroP210 : Registro { public override string REG { get { return "P210"; } } }
    public class RegistroP990 : Registro { public override string REG { get { return "P990"; } } }
    public class Registro1001 : Registro { public override string REG { get { return "1001"; } } }
    public class Registro1010 : Registro { public override string REG { get { return "1010"; } } }
    public class Registro1020 : Registro { public override string REG { get { return "1020"; } } }
    public class Registro1100 : Registro { public override string REG { get { return "1100"; } } }
    public class Registro1101 : Registro { public override string REG { get { return "1101"; } } }
    public class Registro1102 : Registro { public override string REG { get { return "1102"; } } }
    public class Registro1200 : Registro { public override string REG { get { return "1200"; } } }
    public class Registro1210 : Registro { public override string REG { get { return "1210"; } } }
    public class Registro1220 : Registro { public override string REG { get { return "1220"; } } }
    public class Registro1300 : Registro { public override string REG { get { return "1300"; } } }
    public class Registro1500 : Registro { public override string REG { get { return "1500"; } } }
    public class Registro1501 : Registro { public override string REG { get { return "1501"; } } }
    public class Registro1502 : Registro { public override string REG { get { return "1502"; } } }
    public class Registro1600 : Registro { public override string REG { get { return "1600"; } } }
    public class Registro1610 : Registro { public override string REG { get { return "1610"; } } }
    public class Registro1620 : Registro { public override string REG { get { return "1620"; } } }
    public class Registro1700 : Registro { public override string REG { get { return "1700"; } } }
    public class Registro1800 : Registro { public override string REG { get { return "1800"; } } }
    public class Registro1809 : Registro { public override string REG { get { return "1809"; } } }
    public class Registro1900 : Registro { public override string REG { get { return "1900"; } } }
    public class Registro1990 : Registro { public override string REG { get { return "1990"; } } }
    public class Registro9001 : Registro { public override string REG { get { return "9001"; } } }
    public class Registro9900 : Registro { public override string REG { get { return "9900"; } } }
    public class Registro9990 : Registro { public override string REG { get { return "9990"; } } }
    public class Registro9999 : Registro { public override string REG { get { return "9999"; } } }

}
