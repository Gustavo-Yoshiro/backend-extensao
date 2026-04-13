// Generated from c:/Users/kauan/OneDrive/Documentos/GitHub/backend-extensao/Jogo.Core/Linguagem.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue"})
public class LinguagemParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.13.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, TIPO=13, SE=14, ENQUANTO=15, BOOLEANO=16, 
		SOMA=17, SUB=18, MULT=19, DIV=20, MOD=21, E=22, OU=23, IGUAL=24, MENOR=25, 
		MAIOR=26, MAIOR_IGUAL=27, MENOR_IGUAL=28, ID=29, NUMERO_INT=30, NUMERO_FLOAT=31, 
		STRING_LIT=32, WS=33;
	public static final int
		RULE_programa = 0, RULE_comando = 1, RULE_declaracaoVariavel = 2, RULE_atribuicao = 3, 
		RULE_estruturaEnquanto = 4, RULE_estruturaSenao = 5, RULE_estruturaSenaoSe = 6, 
		RULE_estruturaSe = 7, RULE_parametro = 8, RULE_declaracaoFuncao = 9, RULE_comandoRetorno = 10, 
		RULE_chamadaFuncao = 11, RULE_expressao = 12, RULE_lista = 13, RULE_acessoLista = 14;
	private static String[] makeRuleNames() {
		return new String[] {
			"programa", "comando", "declaracaoVariavel", "atribuicao", "estruturaEnquanto", 
			"estruturaSenao", "estruturaSenaoSe", "estruturaSe", "parametro", "declaracaoFuncao", 
			"comandoRetorno", "chamadaFuncao", "expressao", "lista", "acessoLista"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'='", "'('", "')'", "':'", "'fim'", "'senao'", "','", "'funcao'", 
			"'retorna'", "'.'", "'['", "']'", null, "'se'", "'enquanto'", null, "'+'", 
			"'-'", "'*'", "'/'", "'%'", "'e'", "'ou'", "'=='", "'<'", "'>'", "'>='", 
			"'<='"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, "TIPO", "SE", "ENQUANTO", "BOOLEANO", "SOMA", "SUB", "MULT", "DIV", 
			"MOD", "E", "OU", "IGUAL", "MENOR", "MAIOR", "MAIOR_IGUAL", "MENOR_IGUAL", 
			"ID", "NUMERO_INT", "NUMERO_FLOAT", "STRING_LIT", "WS"
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

	@Override
	public String getGrammarFileName() { return "Linguagem.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public LinguagemParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ProgramaContext extends ParserRuleContext {
		public TerminalNode EOF() { return getToken(LinguagemParser.EOF, 0); }
		public List<ComandoContext> comando() {
			return getRuleContexts(ComandoContext.class);
		}
		public ComandoContext comando(int i) {
			return getRuleContext(ComandoContext.class,i);
		}
		public ProgramaContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_programa; }
	}

	public final ProgramaContext programa() throws RecognitionException {
		ProgramaContext _localctx = new ProgramaContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_programa);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(31); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(30);
				comando();
				}
				}
				setState(33); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 536928768L) != 0) );
			setState(35);
			match(EOF);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ComandoContext extends ParserRuleContext {
		public DeclaracaoVariavelContext declaracaoVariavel() {
			return getRuleContext(DeclaracaoVariavelContext.class,0);
		}
		public AtribuicaoContext atribuicao() {
			return getRuleContext(AtribuicaoContext.class,0);
		}
		public EstruturaSeContext estruturaSe() {
			return getRuleContext(EstruturaSeContext.class,0);
		}
		public EstruturaEnquantoContext estruturaEnquanto() {
			return getRuleContext(EstruturaEnquantoContext.class,0);
		}
		public DeclaracaoFuncaoContext declaracaoFuncao() {
			return getRuleContext(DeclaracaoFuncaoContext.class,0);
		}
		public ComandoRetornoContext comandoRetorno() {
			return getRuleContext(ComandoRetornoContext.class,0);
		}
		public ChamadaFuncaoContext chamadaFuncao() {
			return getRuleContext(ChamadaFuncaoContext.class,0);
		}
		public ComandoContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_comando; }
	}

	public final ComandoContext comando() throws RecognitionException {
		ComandoContext _localctx = new ComandoContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_comando);
		try {
			setState(44);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,1,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(37);
				declaracaoVariavel();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(38);
				atribuicao();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(39);
				estruturaSe();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(40);
				estruturaEnquanto();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(41);
				declaracaoFuncao();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(42);
				comandoRetorno();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(43);
				chamadaFuncao();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DeclaracaoVariavelContext extends ParserRuleContext {
		public TerminalNode TIPO() { return getToken(LinguagemParser.TIPO, 0); }
		public TerminalNode ID() { return getToken(LinguagemParser.ID, 0); }
		public ExpressaoContext expressao() {
			return getRuleContext(ExpressaoContext.class,0);
		}
		public DeclaracaoVariavelContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_declaracaoVariavel; }
	}

	public final DeclaracaoVariavelContext declaracaoVariavel() throws RecognitionException {
		DeclaracaoVariavelContext _localctx = new DeclaracaoVariavelContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_declaracaoVariavel);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(46);
			match(TIPO);
			setState(47);
			match(ID);
			setState(48);
			match(T__0);
			setState(49);
			expressao(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class AtribuicaoContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(LinguagemParser.ID, 0); }
		public ExpressaoContext expressao() {
			return getRuleContext(ExpressaoContext.class,0);
		}
		public AtribuicaoContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_atribuicao; }
	}

	public final AtribuicaoContext atribuicao() throws RecognitionException {
		AtribuicaoContext _localctx = new AtribuicaoContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_atribuicao);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(51);
			match(ID);
			setState(52);
			match(T__0);
			setState(53);
			expressao(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EstruturaEnquantoContext extends ParserRuleContext {
		public List<TerminalNode> ENQUANTO() { return getTokens(LinguagemParser.ENQUANTO); }
		public TerminalNode ENQUANTO(int i) {
			return getToken(LinguagemParser.ENQUANTO, i);
		}
		public ExpressaoContext expressao() {
			return getRuleContext(ExpressaoContext.class,0);
		}
		public List<ComandoContext> comando() {
			return getRuleContexts(ComandoContext.class);
		}
		public ComandoContext comando(int i) {
			return getRuleContext(ComandoContext.class,i);
		}
		public EstruturaEnquantoContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_estruturaEnquanto; }
	}

	public final EstruturaEnquantoContext estruturaEnquanto() throws RecognitionException {
		EstruturaEnquantoContext _localctx = new EstruturaEnquantoContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_estruturaEnquanto);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(55);
			match(ENQUANTO);
			setState(56);
			match(T__1);
			setState(57);
			expressao(0);
			setState(58);
			match(T__2);
			setState(59);
			match(T__3);
			setState(61); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(60);
				comando();
				}
				}
				setState(63); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 536928768L) != 0) );
			setState(65);
			match(T__4);
			setState(66);
			match(ENQUANTO);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EstruturaSenaoContext extends ParserRuleContext {
		public List<ComandoContext> comando() {
			return getRuleContexts(ComandoContext.class);
		}
		public ComandoContext comando(int i) {
			return getRuleContext(ComandoContext.class,i);
		}
		public EstruturaSenaoContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_estruturaSenao; }
	}

	public final EstruturaSenaoContext estruturaSenao() throws RecognitionException {
		EstruturaSenaoContext _localctx = new EstruturaSenaoContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_estruturaSenao);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(68);
			match(T__5);
			setState(69);
			match(T__3);
			setState(71); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(70);
				comando();
				}
				}
				setState(73); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 536928768L) != 0) );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EstruturaSenaoSeContext extends ParserRuleContext {
		public TerminalNode SE() { return getToken(LinguagemParser.SE, 0); }
		public ExpressaoContext expressao() {
			return getRuleContext(ExpressaoContext.class,0);
		}
		public List<ComandoContext> comando() {
			return getRuleContexts(ComandoContext.class);
		}
		public ComandoContext comando(int i) {
			return getRuleContext(ComandoContext.class,i);
		}
		public EstruturaSenaoSeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_estruturaSenaoSe; }
	}

	public final EstruturaSenaoSeContext estruturaSenaoSe() throws RecognitionException {
		EstruturaSenaoSeContext _localctx = new EstruturaSenaoSeContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_estruturaSenaoSe);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(75);
			match(T__5);
			setState(76);
			match(SE);
			setState(77);
			match(T__1);
			setState(78);
			expressao(0);
			setState(79);
			match(T__2);
			setState(80);
			match(T__3);
			setState(82); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(81);
				comando();
				}
				}
				setState(84); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 536928768L) != 0) );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class EstruturaSeContext extends ParserRuleContext {
		public List<TerminalNode> SE() { return getTokens(LinguagemParser.SE); }
		public TerminalNode SE(int i) {
			return getToken(LinguagemParser.SE, i);
		}
		public ExpressaoContext expressao() {
			return getRuleContext(ExpressaoContext.class,0);
		}
		public List<ComandoContext> comando() {
			return getRuleContexts(ComandoContext.class);
		}
		public ComandoContext comando(int i) {
			return getRuleContext(ComandoContext.class,i);
		}
		public List<EstruturaSenaoSeContext> estruturaSenaoSe() {
			return getRuleContexts(EstruturaSenaoSeContext.class);
		}
		public EstruturaSenaoSeContext estruturaSenaoSe(int i) {
			return getRuleContext(EstruturaSenaoSeContext.class,i);
		}
		public EstruturaSenaoContext estruturaSenao() {
			return getRuleContext(EstruturaSenaoContext.class,0);
		}
		public EstruturaSeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_estruturaSe; }
	}

	public final EstruturaSeContext estruturaSe() throws RecognitionException {
		EstruturaSeContext _localctx = new EstruturaSeContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_estruturaSe);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(86);
			match(SE);
			setState(87);
			match(T__1);
			setState(88);
			expressao(0);
			setState(89);
			match(T__2);
			setState(90);
			match(T__3);
			setState(92); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(91);
				comando();
				}
				}
				setState(94); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 536928768L) != 0) );
			setState(99);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,6,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(96);
					estruturaSenaoSe();
					}
					} 
				}
				setState(101);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,6,_ctx);
			}
			setState(103);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==T__5) {
				{
				setState(102);
				estruturaSenao();
				}
			}

			setState(105);
			match(T__4);
			setState(106);
			match(SE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ParametroContext extends ParserRuleContext {
		public TerminalNode TIPO() { return getToken(LinguagemParser.TIPO, 0); }
		public TerminalNode ID() { return getToken(LinguagemParser.ID, 0); }
		public ParametroContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_parametro; }
	}

	public final ParametroContext parametro() throws RecognitionException {
		ParametroContext _localctx = new ParametroContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_parametro);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(108);
			match(TIPO);
			setState(109);
			match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DeclaracaoFuncaoContext extends ParserRuleContext {
		public TerminalNode TIPO() { return getToken(LinguagemParser.TIPO, 0); }
		public TerminalNode ID() { return getToken(LinguagemParser.ID, 0); }
		public List<ParametroContext> parametro() {
			return getRuleContexts(ParametroContext.class);
		}
		public ParametroContext parametro(int i) {
			return getRuleContext(ParametroContext.class,i);
		}
		public List<ComandoContext> comando() {
			return getRuleContexts(ComandoContext.class);
		}
		public ComandoContext comando(int i) {
			return getRuleContext(ComandoContext.class,i);
		}
		public DeclaracaoFuncaoContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_declaracaoFuncao; }
	}

	public final DeclaracaoFuncaoContext declaracaoFuncao() throws RecognitionException {
		DeclaracaoFuncaoContext _localctx = new DeclaracaoFuncaoContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_declaracaoFuncao);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(111);
			match(TIPO);
			setState(112);
			match(ID);
			setState(113);
			match(T__1);
			setState(122);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==TIPO) {
				{
				setState(114);
				parametro();
				setState(119);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__6) {
					{
					{
					setState(115);
					match(T__6);
					setState(116);
					parametro();
					}
					}
					setState(121);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(124);
			match(T__2);
			setState(125);
			match(T__3);
			setState(127); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(126);
				comando();
				}
				}
				setState(129); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 536928768L) != 0) );
			setState(131);
			match(T__4);
			setState(132);
			match(T__7);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ComandoRetornoContext extends ParserRuleContext {
		public ExpressaoContext expressao() {
			return getRuleContext(ExpressaoContext.class,0);
		}
		public ComandoRetornoContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_comandoRetorno; }
	}

	public final ComandoRetornoContext comandoRetorno() throws RecognitionException {
		ComandoRetornoContext _localctx = new ComandoRetornoContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_comandoRetorno);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(134);
			match(T__8);
			setState(136);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,11,_ctx) ) {
			case 1:
				{
				setState(135);
				expressao(0);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ChamadaFuncaoContext extends ParserRuleContext {
		public Token objeto;
		public Token funcao;
		public List<TerminalNode> ID() { return getTokens(LinguagemParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(LinguagemParser.ID, i);
		}
		public List<ExpressaoContext> expressao() {
			return getRuleContexts(ExpressaoContext.class);
		}
		public ExpressaoContext expressao(int i) {
			return getRuleContext(ExpressaoContext.class,i);
		}
		public ChamadaFuncaoContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_chamadaFuncao; }
	}

	public final ChamadaFuncaoContext chamadaFuncao() throws RecognitionException {
		ChamadaFuncaoContext _localctx = new ChamadaFuncaoContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_chamadaFuncao);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(140);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,12,_ctx) ) {
			case 1:
				{
				setState(138);
				((ChamadaFuncaoContext)_localctx).objeto = match(ID);
				setState(139);
				match(T__9);
				}
				break;
			}
			setState(142);
			((ChamadaFuncaoContext)_localctx).funcao = match(ID);
			setState(143);
			match(T__1);
			setState(152);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 8053131268L) != 0)) {
				{
				setState(144);
				expressao(0);
				setState(149);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__6) {
					{
					{
					setState(145);
					match(T__6);
					setState(146);
					expressao(0);
					}
					}
					setState(151);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(154);
			match(T__2);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ExpressaoContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(LinguagemParser.ID, 0); }
		public TerminalNode NUMERO_INT() { return getToken(LinguagemParser.NUMERO_INT, 0); }
		public TerminalNode NUMERO_FLOAT() { return getToken(LinguagemParser.NUMERO_FLOAT, 0); }
		public TerminalNode BOOLEANO() { return getToken(LinguagemParser.BOOLEANO, 0); }
		public TerminalNode STRING_LIT() { return getToken(LinguagemParser.STRING_LIT, 0); }
		public ChamadaFuncaoContext chamadaFuncao() {
			return getRuleContext(ChamadaFuncaoContext.class,0);
		}
		public List<ExpressaoContext> expressao() {
			return getRuleContexts(ExpressaoContext.class);
		}
		public ExpressaoContext expressao(int i) {
			return getRuleContext(ExpressaoContext.class,i);
		}
		public ListaContext lista() {
			return getRuleContext(ListaContext.class,0);
		}
		public AcessoListaContext acessoLista() {
			return getRuleContext(AcessoListaContext.class,0);
		}
		public TerminalNode MULT() { return getToken(LinguagemParser.MULT, 0); }
		public TerminalNode DIV() { return getToken(LinguagemParser.DIV, 0); }
		public TerminalNode MOD() { return getToken(LinguagemParser.MOD, 0); }
		public TerminalNode SOMA() { return getToken(LinguagemParser.SOMA, 0); }
		public TerminalNode SUB() { return getToken(LinguagemParser.SUB, 0); }
		public TerminalNode MAIOR() { return getToken(LinguagemParser.MAIOR, 0); }
		public TerminalNode MENOR() { return getToken(LinguagemParser.MENOR, 0); }
		public TerminalNode MAIOR_IGUAL() { return getToken(LinguagemParser.MAIOR_IGUAL, 0); }
		public TerminalNode MENOR_IGUAL() { return getToken(LinguagemParser.MENOR_IGUAL, 0); }
		public TerminalNode IGUAL() { return getToken(LinguagemParser.IGUAL, 0); }
		public TerminalNode E() { return getToken(LinguagemParser.E, 0); }
		public TerminalNode OU() { return getToken(LinguagemParser.OU, 0); }
		public ExpressaoContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expressao; }
	}

	public final ExpressaoContext expressao() throws RecognitionException {
		return expressao(0);
	}

	private ExpressaoContext expressao(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExpressaoContext _localctx = new ExpressaoContext(_ctx, _parentState);
		ExpressaoContext _prevctx = _localctx;
		int _startState = 24;
		enterRecursionRule(_localctx, 24, RULE_expressao, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(169);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,15,_ctx) ) {
			case 1:
				{
				setState(157);
				match(ID);
				}
				break;
			case 2:
				{
				setState(158);
				match(NUMERO_INT);
				}
				break;
			case 3:
				{
				setState(159);
				match(NUMERO_FLOAT);
				}
				break;
			case 4:
				{
				setState(160);
				match(BOOLEANO);
				}
				break;
			case 5:
				{
				setState(161);
				match(STRING_LIT);
				}
				break;
			case 6:
				{
				setState(162);
				chamadaFuncao();
				}
				break;
			case 7:
				{
				setState(163);
				match(T__1);
				setState(164);
				expressao(0);
				setState(165);
				match(T__2);
				}
				break;
			case 8:
				{
				setState(167);
				lista();
				}
				break;
			case 9:
				{
				setState(168);
				acessoLista();
				}
				break;
			}
			_ctx.stop = _input.LT(-1);
			setState(185);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,17,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(183);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,16,_ctx) ) {
					case 1:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(171);
						if (!(precpred(_ctx, 13))) throw new FailedPredicateException(this, "precpred(_ctx, 13)");
						setState(172);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 3670016L) != 0)) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(173);
						expressao(14);
						}
						break;
					case 2:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(174);
						if (!(precpred(_ctx, 12))) throw new FailedPredicateException(this, "precpred(_ctx, 12)");
						setState(175);
						_la = _input.LA(1);
						if ( !(_la==SOMA || _la==SUB) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(176);
						expressao(13);
						}
						break;
					case 3:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(177);
						if (!(precpred(_ctx, 11))) throw new FailedPredicateException(this, "precpred(_ctx, 11)");
						setState(178);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 520093696L) != 0)) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(179);
						expressao(12);
						}
						break;
					case 4:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(180);
						if (!(precpred(_ctx, 10))) throw new FailedPredicateException(this, "precpred(_ctx, 10)");
						setState(181);
						_la = _input.LA(1);
						if ( !(_la==E || _la==OU) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(182);
						expressao(11);
						}
						break;
					}
					} 
				}
				setState(187);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,17,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ListaContext extends ParserRuleContext {
		public List<ExpressaoContext> expressao() {
			return getRuleContexts(ExpressaoContext.class);
		}
		public ExpressaoContext expressao(int i) {
			return getRuleContext(ExpressaoContext.class,i);
		}
		public ListaContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_lista; }
	}

	public final ListaContext lista() throws RecognitionException {
		ListaContext _localctx = new ListaContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_lista);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(188);
			match(T__10);
			setState(197);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 8053131268L) != 0)) {
				{
				setState(189);
				expressao(0);
				setState(194);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__6) {
					{
					{
					setState(190);
					match(T__6);
					setState(191);
					expressao(0);
					}
					}
					setState(196);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(199);
			match(T__11);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class AcessoListaContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(LinguagemParser.ID, 0); }
		public ExpressaoContext expressao() {
			return getRuleContext(ExpressaoContext.class,0);
		}
		public AcessoListaContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_acessoLista; }
	}

	public final AcessoListaContext acessoLista() throws RecognitionException {
		AcessoListaContext _localctx = new AcessoListaContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_acessoLista);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(201);
			match(ID);
			setState(202);
			match(T__10);
			setState(203);
			expressao(0);
			setState(204);
			match(T__11);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 12:
			return expressao_sempred((ExpressaoContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean expressao_sempred(ExpressaoContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 13);
		case 1:
			return precpred(_ctx, 12);
		case 2:
			return precpred(_ctx, 11);
		case 3:
			return precpred(_ctx, 10);
		}
		return true;
	}

	public static final String _serializedATN =
		"\u0004\u0001!\u00cf\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001\u0002"+
		"\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004\u0007\u0004\u0002"+
		"\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007\u0007\u0007\u0002"+
		"\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b\u0007\u000b\u0002"+
		"\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e\u0001\u0000\u0004\u0000"+
		" \b\u0000\u000b\u0000\f\u0000!\u0001\u0000\u0001\u0000\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0003"+
		"\u0001-\b\u0001\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001"+
		"\u0002\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0004\u0001"+
		"\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0004\u0004>\b"+
		"\u0004\u000b\u0004\f\u0004?\u0001\u0004\u0001\u0004\u0001\u0004\u0001"+
		"\u0005\u0001\u0005\u0001\u0005\u0004\u0005H\b\u0005\u000b\u0005\f\u0005"+
		"I\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006"+
		"\u0001\u0006\u0004\u0006S\b\u0006\u000b\u0006\f\u0006T\u0001\u0007\u0001"+
		"\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0004\u0007]\b"+
		"\u0007\u000b\u0007\f\u0007^\u0001\u0007\u0005\u0007b\b\u0007\n\u0007\f"+
		"\u0007e\t\u0007\u0001\u0007\u0003\u0007h\b\u0007\u0001\u0007\u0001\u0007"+
		"\u0001\u0007\u0001\b\u0001\b\u0001\b\u0001\t\u0001\t\u0001\t\u0001\t\u0001"+
		"\t\u0001\t\u0005\tv\b\t\n\t\f\ty\t\t\u0003\t{\b\t\u0001\t\u0001\t\u0001"+
		"\t\u0004\t\u0080\b\t\u000b\t\f\t\u0081\u0001\t\u0001\t\u0001\t\u0001\n"+
		"\u0001\n\u0003\n\u0089\b\n\u0001\u000b\u0001\u000b\u0003\u000b\u008d\b"+
		"\u000b\u0001\u000b\u0001\u000b\u0001\u000b\u0001\u000b\u0001\u000b\u0005"+
		"\u000b\u0094\b\u000b\n\u000b\f\u000b\u0097\t\u000b\u0003\u000b\u0099\b"+
		"\u000b\u0001\u000b\u0001\u000b\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f"+
		"\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f\u0003"+
		"\f\u00aa\b\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001"+
		"\f\u0001\f\u0001\f\u0001\f\u0001\f\u0005\f\u00b8\b\f\n\f\f\f\u00bb\t\f"+
		"\u0001\r\u0001\r\u0001\r\u0001\r\u0005\r\u00c1\b\r\n\r\f\r\u00c4\t\r\u0003"+
		"\r\u00c6\b\r\u0001\r\u0001\r\u0001\u000e\u0001\u000e\u0001\u000e\u0001"+
		"\u000e\u0001\u000e\u0001\u000e\u0000\u0001\u0018\u000f\u0000\u0002\u0004"+
		"\u0006\b\n\f\u000e\u0010\u0012\u0014\u0016\u0018\u001a\u001c\u0000\u0004"+
		"\u0001\u0000\u0013\u0015\u0001\u0000\u0011\u0012\u0001\u0000\u0018\u001c"+
		"\u0001\u0000\u0016\u0017\u00e1\u0000\u001f\u0001\u0000\u0000\u0000\u0002"+
		",\u0001\u0000\u0000\u0000\u0004.\u0001\u0000\u0000\u0000\u00063\u0001"+
		"\u0000\u0000\u0000\b7\u0001\u0000\u0000\u0000\nD\u0001\u0000\u0000\u0000"+
		"\fK\u0001\u0000\u0000\u0000\u000eV\u0001\u0000\u0000\u0000\u0010l\u0001"+
		"\u0000\u0000\u0000\u0012o\u0001\u0000\u0000\u0000\u0014\u0086\u0001\u0000"+
		"\u0000\u0000\u0016\u008c\u0001\u0000\u0000\u0000\u0018\u00a9\u0001\u0000"+
		"\u0000\u0000\u001a\u00bc\u0001\u0000\u0000\u0000\u001c\u00c9\u0001\u0000"+
		"\u0000\u0000\u001e \u0003\u0002\u0001\u0000\u001f\u001e\u0001\u0000\u0000"+
		"\u0000 !\u0001\u0000\u0000\u0000!\u001f\u0001\u0000\u0000\u0000!\"\u0001"+
		"\u0000\u0000\u0000\"#\u0001\u0000\u0000\u0000#$\u0005\u0000\u0000\u0001"+
		"$\u0001\u0001\u0000\u0000\u0000%-\u0003\u0004\u0002\u0000&-\u0003\u0006"+
		"\u0003\u0000\'-\u0003\u000e\u0007\u0000(-\u0003\b\u0004\u0000)-\u0003"+
		"\u0012\t\u0000*-\u0003\u0014\n\u0000+-\u0003\u0016\u000b\u0000,%\u0001"+
		"\u0000\u0000\u0000,&\u0001\u0000\u0000\u0000,\'\u0001\u0000\u0000\u0000"+
		",(\u0001\u0000\u0000\u0000,)\u0001\u0000\u0000\u0000,*\u0001\u0000\u0000"+
		"\u0000,+\u0001\u0000\u0000\u0000-\u0003\u0001\u0000\u0000\u0000./\u0005"+
		"\r\u0000\u0000/0\u0005\u001d\u0000\u000001\u0005\u0001\u0000\u000012\u0003"+
		"\u0018\f\u00002\u0005\u0001\u0000\u0000\u000034\u0005\u001d\u0000\u0000"+
		"45\u0005\u0001\u0000\u000056\u0003\u0018\f\u00006\u0007\u0001\u0000\u0000"+
		"\u000078\u0005\u000f\u0000\u000089\u0005\u0002\u0000\u00009:\u0003\u0018"+
		"\f\u0000:;\u0005\u0003\u0000\u0000;=\u0005\u0004\u0000\u0000<>\u0003\u0002"+
		"\u0001\u0000=<\u0001\u0000\u0000\u0000>?\u0001\u0000\u0000\u0000?=\u0001"+
		"\u0000\u0000\u0000?@\u0001\u0000\u0000\u0000@A\u0001\u0000\u0000\u0000"+
		"AB\u0005\u0005\u0000\u0000BC\u0005\u000f\u0000\u0000C\t\u0001\u0000\u0000"+
		"\u0000DE\u0005\u0006\u0000\u0000EG\u0005\u0004\u0000\u0000FH\u0003\u0002"+
		"\u0001\u0000GF\u0001\u0000\u0000\u0000HI\u0001\u0000\u0000\u0000IG\u0001"+
		"\u0000\u0000\u0000IJ\u0001\u0000\u0000\u0000J\u000b\u0001\u0000\u0000"+
		"\u0000KL\u0005\u0006\u0000\u0000LM\u0005\u000e\u0000\u0000MN\u0005\u0002"+
		"\u0000\u0000NO\u0003\u0018\f\u0000OP\u0005\u0003\u0000\u0000PR\u0005\u0004"+
		"\u0000\u0000QS\u0003\u0002\u0001\u0000RQ\u0001\u0000\u0000\u0000ST\u0001"+
		"\u0000\u0000\u0000TR\u0001\u0000\u0000\u0000TU\u0001\u0000\u0000\u0000"+
		"U\r\u0001\u0000\u0000\u0000VW\u0005\u000e\u0000\u0000WX\u0005\u0002\u0000"+
		"\u0000XY\u0003\u0018\f\u0000YZ\u0005\u0003\u0000\u0000Z\\\u0005\u0004"+
		"\u0000\u0000[]\u0003\u0002\u0001\u0000\\[\u0001\u0000\u0000\u0000]^\u0001"+
		"\u0000\u0000\u0000^\\\u0001\u0000\u0000\u0000^_\u0001\u0000\u0000\u0000"+
		"_c\u0001\u0000\u0000\u0000`b\u0003\f\u0006\u0000a`\u0001\u0000\u0000\u0000"+
		"be\u0001\u0000\u0000\u0000ca\u0001\u0000\u0000\u0000cd\u0001\u0000\u0000"+
		"\u0000dg\u0001\u0000\u0000\u0000ec\u0001\u0000\u0000\u0000fh\u0003\n\u0005"+
		"\u0000gf\u0001\u0000\u0000\u0000gh\u0001\u0000\u0000\u0000hi\u0001\u0000"+
		"\u0000\u0000ij\u0005\u0005\u0000\u0000jk\u0005\u000e\u0000\u0000k\u000f"+
		"\u0001\u0000\u0000\u0000lm\u0005\r\u0000\u0000mn\u0005\u001d\u0000\u0000"+
		"n\u0011\u0001\u0000\u0000\u0000op\u0005\r\u0000\u0000pq\u0005\u001d\u0000"+
		"\u0000qz\u0005\u0002\u0000\u0000rw\u0003\u0010\b\u0000st\u0005\u0007\u0000"+
		"\u0000tv\u0003\u0010\b\u0000us\u0001\u0000\u0000\u0000vy\u0001\u0000\u0000"+
		"\u0000wu\u0001\u0000\u0000\u0000wx\u0001\u0000\u0000\u0000x{\u0001\u0000"+
		"\u0000\u0000yw\u0001\u0000\u0000\u0000zr\u0001\u0000\u0000\u0000z{\u0001"+
		"\u0000\u0000\u0000{|\u0001\u0000\u0000\u0000|}\u0005\u0003\u0000\u0000"+
		"}\u007f\u0005\u0004\u0000\u0000~\u0080\u0003\u0002\u0001\u0000\u007f~"+
		"\u0001\u0000\u0000\u0000\u0080\u0081\u0001\u0000\u0000\u0000\u0081\u007f"+
		"\u0001\u0000\u0000\u0000\u0081\u0082\u0001\u0000\u0000\u0000\u0082\u0083"+
		"\u0001\u0000\u0000\u0000\u0083\u0084\u0005\u0005\u0000\u0000\u0084\u0085"+
		"\u0005\b\u0000\u0000\u0085\u0013\u0001\u0000\u0000\u0000\u0086\u0088\u0005"+
		"\t\u0000\u0000\u0087\u0089\u0003\u0018\f\u0000\u0088\u0087\u0001\u0000"+
		"\u0000\u0000\u0088\u0089\u0001\u0000\u0000\u0000\u0089\u0015\u0001\u0000"+
		"\u0000\u0000\u008a\u008b\u0005\u001d\u0000\u0000\u008b\u008d\u0005\n\u0000"+
		"\u0000\u008c\u008a\u0001\u0000\u0000\u0000\u008c\u008d\u0001\u0000\u0000"+
		"\u0000\u008d\u008e\u0001\u0000\u0000\u0000\u008e\u008f\u0005\u001d\u0000"+
		"\u0000\u008f\u0098\u0005\u0002\u0000\u0000\u0090\u0095\u0003\u0018\f\u0000"+
		"\u0091\u0092\u0005\u0007\u0000\u0000\u0092\u0094\u0003\u0018\f\u0000\u0093"+
		"\u0091\u0001\u0000\u0000\u0000\u0094\u0097\u0001\u0000\u0000\u0000\u0095"+
		"\u0093\u0001\u0000\u0000\u0000\u0095\u0096\u0001\u0000\u0000\u0000\u0096"+
		"\u0099\u0001\u0000\u0000\u0000\u0097\u0095\u0001\u0000\u0000\u0000\u0098"+
		"\u0090\u0001\u0000\u0000\u0000\u0098\u0099\u0001\u0000\u0000\u0000\u0099"+
		"\u009a\u0001\u0000\u0000\u0000\u009a\u009b\u0005\u0003\u0000\u0000\u009b"+
		"\u0017\u0001\u0000\u0000\u0000\u009c\u009d\u0006\f\uffff\uffff\u0000\u009d"+
		"\u00aa\u0005\u001d\u0000\u0000\u009e\u00aa\u0005\u001e\u0000\u0000\u009f"+
		"\u00aa\u0005\u001f\u0000\u0000\u00a0\u00aa\u0005\u0010\u0000\u0000\u00a1"+
		"\u00aa\u0005 \u0000\u0000\u00a2\u00aa\u0003\u0016\u000b\u0000\u00a3\u00a4"+
		"\u0005\u0002\u0000\u0000\u00a4\u00a5\u0003\u0018\f\u0000\u00a5\u00a6\u0005"+
		"\u0003\u0000\u0000\u00a6\u00aa\u0001\u0000\u0000\u0000\u00a7\u00aa\u0003"+
		"\u001a\r\u0000\u00a8\u00aa\u0003\u001c\u000e\u0000\u00a9\u009c\u0001\u0000"+
		"\u0000\u0000\u00a9\u009e\u0001\u0000\u0000\u0000\u00a9\u009f\u0001\u0000"+
		"\u0000\u0000\u00a9\u00a0\u0001\u0000\u0000\u0000\u00a9\u00a1\u0001\u0000"+
		"\u0000\u0000\u00a9\u00a2\u0001\u0000\u0000\u0000\u00a9\u00a3\u0001\u0000"+
		"\u0000\u0000\u00a9\u00a7\u0001\u0000\u0000\u0000\u00a9\u00a8\u0001\u0000"+
		"\u0000\u0000\u00aa\u00b9\u0001\u0000\u0000\u0000\u00ab\u00ac\n\r\u0000"+
		"\u0000\u00ac\u00ad\u0007\u0000\u0000\u0000\u00ad\u00b8\u0003\u0018\f\u000e"+
		"\u00ae\u00af\n\f\u0000\u0000\u00af\u00b0\u0007\u0001\u0000\u0000\u00b0"+
		"\u00b8\u0003\u0018\f\r\u00b1\u00b2\n\u000b\u0000\u0000\u00b2\u00b3\u0007"+
		"\u0002\u0000\u0000\u00b3\u00b8\u0003\u0018\f\f\u00b4\u00b5\n\n\u0000\u0000"+
		"\u00b5\u00b6\u0007\u0003\u0000\u0000\u00b6\u00b8\u0003\u0018\f\u000b\u00b7"+
		"\u00ab\u0001\u0000\u0000\u0000\u00b7\u00ae\u0001\u0000\u0000\u0000\u00b7"+
		"\u00b1\u0001\u0000\u0000\u0000\u00b7\u00b4\u0001\u0000\u0000\u0000\u00b8"+
		"\u00bb\u0001\u0000\u0000\u0000\u00b9\u00b7\u0001\u0000\u0000\u0000\u00b9"+
		"\u00ba\u0001\u0000\u0000\u0000\u00ba\u0019\u0001\u0000\u0000\u0000\u00bb"+
		"\u00b9\u0001\u0000\u0000\u0000\u00bc\u00c5\u0005\u000b\u0000\u0000\u00bd"+
		"\u00c2\u0003\u0018\f\u0000\u00be\u00bf\u0005\u0007\u0000\u0000\u00bf\u00c1"+
		"\u0003\u0018\f\u0000\u00c0\u00be\u0001\u0000\u0000\u0000\u00c1\u00c4\u0001"+
		"\u0000\u0000\u0000\u00c2\u00c0\u0001\u0000\u0000\u0000\u00c2\u00c3\u0001"+
		"\u0000\u0000\u0000\u00c3\u00c6\u0001\u0000\u0000\u0000\u00c4\u00c2\u0001"+
		"\u0000\u0000\u0000\u00c5\u00bd\u0001\u0000\u0000\u0000\u00c5\u00c6\u0001"+
		"\u0000\u0000\u0000\u00c6\u00c7\u0001\u0000\u0000\u0000\u00c7\u00c8\u0005"+
		"\f\u0000\u0000\u00c8\u001b\u0001\u0000\u0000\u0000\u00c9\u00ca\u0005\u001d"+
		"\u0000\u0000\u00ca\u00cb\u0005\u000b\u0000\u0000\u00cb\u00cc\u0003\u0018"+
		"\f\u0000\u00cc\u00cd\u0005\f\u0000\u0000\u00cd\u001d\u0001\u0000\u0000"+
		"\u0000\u0014!,?IT^cgwz\u0081\u0088\u008c\u0095\u0098\u00a9\u00b7\u00b9"+
		"\u00c2\u00c5";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}