// Generated from c:/Users/kauan/OneDrive/Documentos/GitHub/backend-extensao/Jogo.Core/Linguagem.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue", "this-escape"})
public class LinguagemLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.13.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, TIPO=8, SE=9, 
		ENQUANTO=10, BOOLEANO=11, SOMA=12, SUB=13, MULT=14, DIV=15, MOD=16, E=17, 
		OU=18, IGUAL=19, MAIOR_IGUAL=20, MENOR_IGUAL=21, ID=22, NUMERO_INT=23, 
		NUMERO_FLOAT=24, STRING_LIT=25, WS=26;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "TIPO", "SE", 
			"ENQUANTO", "BOOLEANO", "SOMA", "SUB", "MULT", "DIV", "MOD", "E", "OU", 
			"IGUAL", "MAIOR_IGUAL", "MENOR_IGUAL", "ID", "NUMERO_INT", "NUMERO_FLOAT", 
			"STRING_LIT", "WS"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'='", "'('", "')'", "':'", "','", "'['", "']'", null, "'se'", 
			"'enquanto'", null, "'+'", "'-'", "'*'", "'/'", "'%'", "'e'", "'ou'", 
			"'=='", "'>='", "'<='"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, "TIPO", "SE", "ENQUANTO", 
			"BOOLEANO", "SOMA", "SUB", "MULT", "DIV", "MOD", "E", "OU", "IGUAL", 
			"MAIOR_IGUAL", "MENOR_IGUAL", "ID", "NUMERO_INT", "NUMERO_FLOAT", "STRING_LIT", 
			"WS"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}


	public LinguagemLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "Linguagem.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\u0004\u0000\u001a\u00b3\u0006\uffff\uffff\u0002\u0000\u0007\u0000\u0002"+
		"\u0001\u0007\u0001\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002"+
		"\u0004\u0007\u0004\u0002\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002"+
		"\u0007\u0007\u0007\u0002\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002"+
		"\u000b\u0007\u000b\u0002\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e"+
		"\u0002\u000f\u0007\u000f\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011"+
		"\u0002\u0012\u0007\u0012\u0002\u0013\u0007\u0013\u0002\u0014\u0007\u0014"+
		"\u0002\u0015\u0007\u0015\u0002\u0016\u0007\u0016\u0002\u0017\u0007\u0017"+
		"\u0002\u0018\u0007\u0018\u0002\u0019\u0007\u0019\u0001\u0000\u0001\u0000"+
		"\u0001\u0001\u0001\u0001\u0001\u0002\u0001\u0002\u0001\u0003\u0001\u0003"+
		"\u0001\u0004\u0001\u0004\u0001\u0005\u0001\u0005\u0001\u0006\u0001\u0006"+
		"\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007"+
		"\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007"+
		"\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007"+
		"\u0003\u0007V\b\u0007\u0001\b\u0001\b\u0001\b\u0001\t\u0001\t\u0001\t"+
		"\u0001\t\u0001\t\u0001\t\u0001\t\u0001\t\u0001\t\u0001\n\u0001\n\u0001"+
		"\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001"+
		"\n\u0001\n\u0001\n\u0001\n\u0003\ns\b\n\u0001\u000b\u0001\u000b\u0001"+
		"\f\u0001\f\u0001\r\u0001\r\u0001\u000e\u0001\u000e\u0001\u000f\u0001\u000f"+
		"\u0001\u0010\u0001\u0010\u0001\u0011\u0001\u0011\u0001\u0011\u0001\u0012"+
		"\u0001\u0012\u0001\u0012\u0001\u0013\u0001\u0013\u0001\u0013\u0001\u0014"+
		"\u0001\u0014\u0001\u0014\u0001\u0015\u0001\u0015\u0005\u0015\u008f\b\u0015"+
		"\n\u0015\f\u0015\u0092\t\u0015\u0001\u0016\u0004\u0016\u0095\b\u0016\u000b"+
		"\u0016\f\u0016\u0096\u0001\u0017\u0004\u0017\u009a\b\u0017\u000b\u0017"+
		"\f\u0017\u009b\u0001\u0017\u0001\u0017\u0004\u0017\u00a0\b\u0017\u000b"+
		"\u0017\f\u0017\u00a1\u0001\u0018\u0001\u0018\u0005\u0018\u00a6\b\u0018"+
		"\n\u0018\f\u0018\u00a9\t\u0018\u0001\u0018\u0001\u0018\u0001\u0019\u0004"+
		"\u0019\u00ae\b\u0019\u000b\u0019\f\u0019\u00af\u0001\u0019\u0001\u0019"+
		"\u0000\u0000\u001a\u0001\u0001\u0003\u0002\u0005\u0003\u0007\u0004\t\u0005"+
		"\u000b\u0006\r\u0007\u000f\b\u0011\t\u0013\n\u0015\u000b\u0017\f\u0019"+
		"\r\u001b\u000e\u001d\u000f\u001f\u0010!\u0011#\u0012%\u0013\'\u0014)\u0015"+
		"+\u0016-\u0017/\u00181\u00193\u001a\u0001\u0000\u0005\u0003\u0000AZ__"+
		"az\u0004\u000009AZ__az\u0001\u000009\u0001\u0000\"\"\u0003\u0000\t\n\r"+
		"\r  \u00bc\u0000\u0001\u0001\u0000\u0000\u0000\u0000\u0003\u0001\u0000"+
		"\u0000\u0000\u0000\u0005\u0001\u0000\u0000\u0000\u0000\u0007\u0001\u0000"+
		"\u0000\u0000\u0000\t\u0001\u0000\u0000\u0000\u0000\u000b\u0001\u0000\u0000"+
		"\u0000\u0000\r\u0001\u0000\u0000\u0000\u0000\u000f\u0001\u0000\u0000\u0000"+
		"\u0000\u0011\u0001\u0000\u0000\u0000\u0000\u0013\u0001\u0000\u0000\u0000"+
		"\u0000\u0015\u0001\u0000\u0000\u0000\u0000\u0017\u0001\u0000\u0000\u0000"+
		"\u0000\u0019\u0001\u0000\u0000\u0000\u0000\u001b\u0001\u0000\u0000\u0000"+
		"\u0000\u001d\u0001\u0000\u0000\u0000\u0000\u001f\u0001\u0000\u0000\u0000"+
		"\u0000!\u0001\u0000\u0000\u0000\u0000#\u0001\u0000\u0000\u0000\u0000%"+
		"\u0001\u0000\u0000\u0000\u0000\'\u0001\u0000\u0000\u0000\u0000)\u0001"+
		"\u0000\u0000\u0000\u0000+\u0001\u0000\u0000\u0000\u0000-\u0001\u0000\u0000"+
		"\u0000\u0000/\u0001\u0000\u0000\u0000\u00001\u0001\u0000\u0000\u0000\u0000"+
		"3\u0001\u0000\u0000\u0000\u00015\u0001\u0000\u0000\u0000\u00037\u0001"+
		"\u0000\u0000\u0000\u00059\u0001\u0000\u0000\u0000\u0007;\u0001\u0000\u0000"+
		"\u0000\t=\u0001\u0000\u0000\u0000\u000b?\u0001\u0000\u0000\u0000\rA\u0001"+
		"\u0000\u0000\u0000\u000fU\u0001\u0000\u0000\u0000\u0011W\u0001\u0000\u0000"+
		"\u0000\u0013Z\u0001\u0000\u0000\u0000\u0015r\u0001\u0000\u0000\u0000\u0017"+
		"t\u0001\u0000\u0000\u0000\u0019v\u0001\u0000\u0000\u0000\u001bx\u0001"+
		"\u0000\u0000\u0000\u001dz\u0001\u0000\u0000\u0000\u001f|\u0001\u0000\u0000"+
		"\u0000!~\u0001\u0000\u0000\u0000#\u0080\u0001\u0000\u0000\u0000%\u0083"+
		"\u0001\u0000\u0000\u0000\'\u0086\u0001\u0000\u0000\u0000)\u0089\u0001"+
		"\u0000\u0000\u0000+\u008c\u0001\u0000\u0000\u0000-\u0094\u0001\u0000\u0000"+
		"\u0000/\u0099\u0001\u0000\u0000\u00001\u00a3\u0001\u0000\u0000\u00003"+
		"\u00ad\u0001\u0000\u0000\u000056\u0005=\u0000\u00006\u0002\u0001\u0000"+
		"\u0000\u000078\u0005(\u0000\u00008\u0004\u0001\u0000\u0000\u00009:\u0005"+
		")\u0000\u0000:\u0006\u0001\u0000\u0000\u0000;<\u0005:\u0000\u0000<\b\u0001"+
		"\u0000\u0000\u0000=>\u0005,\u0000\u0000>\n\u0001\u0000\u0000\u0000?@\u0005"+
		"[\u0000\u0000@\f\u0001\u0000\u0000\u0000AB\u0005]\u0000\u0000B\u000e\u0001"+
		"\u0000\u0000\u0000CD\u0005i\u0000\u0000DE\u0005n\u0000\u0000EV\u0005t"+
		"\u0000\u0000FG\u0005f\u0000\u0000GH\u0005l\u0000\u0000HI\u0005o\u0000"+
		"\u0000IJ\u0005a\u0000\u0000JV\u0005t\u0000\u0000KL\u0005b\u0000\u0000"+
		"LM\u0005o\u0000\u0000MN\u0005o\u0000\u0000NV\u0005l\u0000\u0000OP\u0005"+
		"s\u0000\u0000PQ\u0005t\u0000\u0000QR\u0005r\u0000\u0000RS\u0005i\u0000"+
		"\u0000ST\u0005n\u0000\u0000TV\u0005g\u0000\u0000UC\u0001\u0000\u0000\u0000"+
		"UF\u0001\u0000\u0000\u0000UK\u0001\u0000\u0000\u0000UO\u0001\u0000\u0000"+
		"\u0000V\u0010\u0001\u0000\u0000\u0000WX\u0005s\u0000\u0000XY\u0005e\u0000"+
		"\u0000Y\u0012\u0001\u0000\u0000\u0000Z[\u0005e\u0000\u0000[\\\u0005n\u0000"+
		"\u0000\\]\u0005q\u0000\u0000]^\u0005u\u0000\u0000^_\u0005a\u0000\u0000"+
		"_`\u0005n\u0000\u0000`a\u0005t\u0000\u0000ab\u0005o\u0000\u0000b\u0014"+
		"\u0001\u0000\u0000\u0000cd\u0005V\u0000\u0000de\u0005e\u0000\u0000ef\u0005"+
		"r\u0000\u0000fg\u0005d\u0000\u0000gh\u0005a\u0000\u0000hi\u0005d\u0000"+
		"\u0000ij\u0005e\u0000\u0000jk\u0005i\u0000\u0000kl\u0005r\u0000\u0000"+
		"ls\u0005o\u0000\u0000mn\u0005F\u0000\u0000no\u0005a\u0000\u0000op\u0005"+
		"l\u0000\u0000pq\u0005s\u0000\u0000qs\u0005o\u0000\u0000rc\u0001\u0000"+
		"\u0000\u0000rm\u0001\u0000\u0000\u0000s\u0016\u0001\u0000\u0000\u0000"+
		"tu\u0005+\u0000\u0000u\u0018\u0001\u0000\u0000\u0000vw\u0005-\u0000\u0000"+
		"w\u001a\u0001\u0000\u0000\u0000xy\u0005*\u0000\u0000y\u001c\u0001\u0000"+
		"\u0000\u0000z{\u0005/\u0000\u0000{\u001e\u0001\u0000\u0000\u0000|}\u0005"+
		"%\u0000\u0000} \u0001\u0000\u0000\u0000~\u007f\u0005e\u0000\u0000\u007f"+
		"\"\u0001\u0000\u0000\u0000\u0080\u0081\u0005o\u0000\u0000\u0081\u0082"+
		"\u0005u\u0000\u0000\u0082$\u0001\u0000\u0000\u0000\u0083\u0084\u0005="+
		"\u0000\u0000\u0084\u0085\u0005=\u0000\u0000\u0085&\u0001\u0000\u0000\u0000"+
		"\u0086\u0087\u0005>\u0000\u0000\u0087\u0088\u0005=\u0000\u0000\u0088("+
		"\u0001\u0000\u0000\u0000\u0089\u008a\u0005<\u0000\u0000\u008a\u008b\u0005"+
		"=\u0000\u0000\u008b*\u0001\u0000\u0000\u0000\u008c\u0090\u0007\u0000\u0000"+
		"\u0000\u008d\u008f\u0007\u0001\u0000\u0000\u008e\u008d\u0001\u0000\u0000"+
		"\u0000\u008f\u0092\u0001\u0000\u0000\u0000\u0090\u008e\u0001\u0000\u0000"+
		"\u0000\u0090\u0091\u0001\u0000\u0000\u0000\u0091,\u0001\u0000\u0000\u0000"+
		"\u0092\u0090\u0001\u0000\u0000\u0000\u0093\u0095\u0007\u0002\u0000\u0000"+
		"\u0094\u0093\u0001\u0000\u0000\u0000\u0095\u0096\u0001\u0000\u0000\u0000"+
		"\u0096\u0094\u0001\u0000\u0000\u0000\u0096\u0097\u0001\u0000\u0000\u0000"+
		"\u0097.\u0001\u0000\u0000\u0000\u0098\u009a\u0007\u0002\u0000\u0000\u0099"+
		"\u0098\u0001\u0000\u0000\u0000\u009a\u009b\u0001\u0000\u0000\u0000\u009b"+
		"\u0099\u0001\u0000\u0000\u0000\u009b\u009c\u0001\u0000\u0000\u0000\u009c"+
		"\u009d\u0001\u0000\u0000\u0000\u009d\u009f\u0005.\u0000\u0000\u009e\u00a0"+
		"\u0007\u0002\u0000\u0000\u009f\u009e\u0001\u0000\u0000\u0000\u00a0\u00a1"+
		"\u0001\u0000\u0000\u0000\u00a1\u009f\u0001\u0000\u0000\u0000\u00a1\u00a2"+
		"\u0001\u0000\u0000\u0000\u00a20\u0001\u0000\u0000\u0000\u00a3\u00a7\u0005"+
		"\"\u0000\u0000\u00a4\u00a6\b\u0003\u0000\u0000\u00a5\u00a4\u0001\u0000"+
		"\u0000\u0000\u00a6\u00a9\u0001\u0000\u0000\u0000\u00a7\u00a5\u0001\u0000"+
		"\u0000\u0000\u00a7\u00a8\u0001\u0000\u0000\u0000\u00a8\u00aa\u0001\u0000"+
		"\u0000\u0000\u00a9\u00a7\u0001\u0000\u0000\u0000\u00aa\u00ab\u0005\"\u0000"+
		"\u0000\u00ab2\u0001\u0000\u0000\u0000\u00ac\u00ae\u0007\u0004\u0000\u0000"+
		"\u00ad\u00ac\u0001\u0000\u0000\u0000\u00ae\u00af\u0001\u0000\u0000\u0000"+
		"\u00af\u00ad\u0001\u0000\u0000\u0000\u00af\u00b0\u0001\u0000\u0000\u0000"+
		"\u00b0\u00b1\u0001\u0000\u0000\u0000\u00b1\u00b2\u0006\u0019\u0000\u0000"+
		"\u00b24\u0001\u0000\u0000\u0000\t\u0000Ur\u0090\u0096\u009b\u00a1\u00a7"+
		"\u00af\u0001\u0006\u0000\u0000";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}