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
		T__9=10, TIPO=11, SE=12, ENQUANTO=13, BOOLEANO=14, SOMA=15, SUB=16, MULT=17, 
		DIV=18, MOD=19, E=20, OU=21, IGUAL=22, MENOR=23, MAIOR=24, MAIOR_IGUAL=25, 
		MENOR_IGUAL=26, ID=27, NUMERO_INT=28, NUMERO_FLOAT=29, STRING_LIT=30, 
		WS=31;
	public static final int
		RULE_programa = 0, RULE_comando = 1, RULE_declaracaoVariavel = 2, RULE_atribuicao = 3, 
		RULE_estruturaSe = 4, RULE_estruturaSenao = 5, RULE_estruturaEnquanto = 6, 
		RULE_chamadaFuncao = 7, RULE_expressao = 8, RULE_lista = 9, RULE_acessoLista = 10;
	private static String[] makeRuleNames() {
		return new String[] {
			"programa", "comando", "declaracaoVariavel", "atribuicao", "estruturaSe", 
			"estruturaSenao", "estruturaEnquanto", "chamadaFuncao", "expressao", 
			"lista", "acessoLista"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'='", "'('", "')'", "':'", "'fim'", "'senao'", "'.'", "','", "'['", 
			"']'", null, "'se'", "'enquanto'", null, "'+'", "'-'", "'*'", "'/'", 
			"'%'", "'e'", "'ou'", "'=='", "'<'", "'>'", "'>='", "'<='"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, null, null, null, "TIPO", 
			"SE", "ENQUANTO", "BOOLEANO", "SOMA", "SUB", "MULT", "DIV", "MOD", "E", 
			"OU", "IGUAL", "MENOR", "MAIOR", "MAIOR_IGUAL", "MENOR_IGUAL", "ID", 
			"NUMERO_INT", "NUMERO_FLOAT", "STRING_LIT", "WS"
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
			setState(23); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(22);
				comando();
				}
				}
				setState(25); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 134232064L) != 0) );
			setState(27);
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
			setState(34);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,1,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(29);
				declaracaoVariavel();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(30);
				atribuicao();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(31);
				estruturaSe();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(32);
				estruturaEnquanto();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(33);
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
			setState(36);
			match(TIPO);
			setState(37);
			match(ID);
			setState(38);
			match(T__0);
			setState(39);
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
			setState(41);
			match(ID);
			setState(42);
			match(T__0);
			setState(43);
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
		enterRule(_localctx, 8, RULE_estruturaSe);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(45);
			match(SE);
			setState(46);
			match(T__1);
			setState(47);
			expressao(0);
			setState(48);
			match(T__2);
			setState(49);
			match(T__3);
			setState(51); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(50);
				comando();
				}
				}
				setState(53); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 134232064L) != 0) );
			setState(56);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==T__5) {
				{
				setState(55);
				estruturaSenao();
				}
			}

			setState(58);
			match(T__4);
			setState(59);
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
			setState(61);
			match(T__5);
			setState(62);
			match(T__3);
			setState(64); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(63);
				comando();
				}
				}
				setState(66); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 134232064L) != 0) );
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
		enterRule(_localctx, 12, RULE_estruturaEnquanto);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(68);
			match(ENQUANTO);
			setState(69);
			match(T__1);
			setState(70);
			expressao(0);
			setState(71);
			match(T__2);
			setState(72);
			match(T__3);
			setState(74); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(73);
				comando();
				}
				}
				setState(76); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 134232064L) != 0) );
			setState(78);
			match(T__4);
			setState(79);
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
		enterRule(_localctx, 14, RULE_chamadaFuncao);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(83);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,6,_ctx) ) {
			case 1:
				{
				setState(81);
				((ChamadaFuncaoContext)_localctx).objeto = match(ID);
				setState(82);
				match(T__6);
				}
				break;
			}
			setState(85);
			((ChamadaFuncaoContext)_localctx).funcao = match(ID);
			setState(86);
			match(T__1);
			setState(95);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 2013282820L) != 0)) {
				{
				setState(87);
				expressao(0);
				setState(92);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__7) {
					{
					{
					setState(88);
					match(T__7);
					setState(89);
					expressao(0);
					}
					}
					setState(94);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(97);
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
		int _startState = 16;
		enterRecursionRule(_localctx, 16, RULE_expressao, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(112);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,9,_ctx) ) {
			case 1:
				{
				setState(100);
				match(ID);
				}
				break;
			case 2:
				{
				setState(101);
				match(NUMERO_INT);
				}
				break;
			case 3:
				{
				setState(102);
				match(NUMERO_FLOAT);
				}
				break;
			case 4:
				{
				setState(103);
				match(BOOLEANO);
				}
				break;
			case 5:
				{
				setState(104);
				match(STRING_LIT);
				}
				break;
			case 6:
				{
				setState(105);
				chamadaFuncao();
				}
				break;
			case 7:
				{
				setState(106);
				match(T__1);
				setState(107);
				expressao(0);
				setState(108);
				match(T__2);
				}
				break;
			case 8:
				{
				setState(110);
				lista();
				}
				break;
			case 9:
				{
				setState(111);
				acessoLista();
				}
				break;
			}
			_ctx.stop = _input.LT(-1);
			setState(128);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,11,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(126);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,10,_ctx) ) {
					case 1:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(114);
						if (!(precpred(_ctx, 13))) throw new FailedPredicateException(this, "precpred(_ctx, 13)");
						setState(115);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 917504L) != 0)) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(116);
						expressao(14);
						}
						break;
					case 2:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(117);
						if (!(precpred(_ctx, 12))) throw new FailedPredicateException(this, "precpred(_ctx, 12)");
						setState(118);
						_la = _input.LA(1);
						if ( !(_la==SOMA || _la==SUB) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(119);
						expressao(13);
						}
						break;
					case 3:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(120);
						if (!(precpred(_ctx, 11))) throw new FailedPredicateException(this, "precpred(_ctx, 11)");
						setState(121);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 130023424L) != 0)) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(122);
						expressao(12);
						}
						break;
					case 4:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(123);
						if (!(precpred(_ctx, 10))) throw new FailedPredicateException(this, "precpred(_ctx, 10)");
						setState(124);
						_la = _input.LA(1);
						if ( !(_la==E || _la==OU) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(125);
						expressao(11);
						}
						break;
					}
					} 
				}
				setState(130);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,11,_ctx);
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
		enterRule(_localctx, 18, RULE_lista);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(131);
			match(T__8);
			setState(140);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 2013282820L) != 0)) {
				{
				setState(132);
				expressao(0);
				setState(137);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__7) {
					{
					{
					setState(133);
					match(T__7);
					setState(134);
					expressao(0);
					}
					}
					setState(139);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(142);
			match(T__9);
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
		enterRule(_localctx, 20, RULE_acessoLista);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(144);
			match(ID);
			setState(145);
			match(T__8);
			setState(146);
			expressao(0);
			setState(147);
			match(T__9);
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
		case 8:
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
		"\u0004\u0001\u001f\u0096\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001"+
		"\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004\u0007\u0004"+
		"\u0002\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007\u0007\u0007"+
		"\u0002\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0001\u0000\u0004\u0000"+
		"\u0018\b\u0000\u000b\u0000\f\u0000\u0019\u0001\u0000\u0001\u0000\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0003\u0001#\b"+
		"\u0001\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001"+
		"\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0004\u0001\u0004\u0001"+
		"\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0004\u00044\b\u0004\u000b"+
		"\u0004\f\u00045\u0001\u0004\u0003\u00049\b\u0004\u0001\u0004\u0001\u0004"+
		"\u0001\u0004\u0001\u0005\u0001\u0005\u0001\u0005\u0004\u0005A\b\u0005"+
		"\u000b\u0005\f\u0005B\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006"+
		"\u0001\u0006\u0001\u0006\u0004\u0006K\b\u0006\u000b\u0006\f\u0006L\u0001"+
		"\u0006\u0001\u0006\u0001\u0006\u0001\u0007\u0001\u0007\u0003\u0007T\b"+
		"\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0005"+
		"\u0007[\b\u0007\n\u0007\f\u0007^\t\u0007\u0003\u0007`\b\u0007\u0001\u0007"+
		"\u0001\u0007\u0001\b\u0001\b\u0001\b\u0001\b\u0001\b\u0001\b\u0001\b\u0001"+
		"\b\u0001\b\u0001\b\u0001\b\u0001\b\u0001\b\u0003\bq\b\b\u0001\b\u0001"+
		"\b\u0001\b\u0001\b\u0001\b\u0001\b\u0001\b\u0001\b\u0001\b\u0001\b\u0001"+
		"\b\u0001\b\u0005\b\u007f\b\b\n\b\f\b\u0082\t\b\u0001\t\u0001\t\u0001\t"+
		"\u0001\t\u0005\t\u0088\b\t\n\t\f\t\u008b\t\t\u0003\t\u008d\b\t\u0001\t"+
		"\u0001\t\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0000\u0001\u0010"+
		"\u000b\u0000\u0002\u0004\u0006\b\n\f\u000e\u0010\u0012\u0014\u0000\u0004"+
		"\u0001\u0000\u0011\u0013\u0001\u0000\u000f\u0010\u0001\u0000\u0016\u001a"+
		"\u0001\u0000\u0014\u0015\u00a4\u0000\u0017\u0001\u0000\u0000\u0000\u0002"+
		"\"\u0001\u0000\u0000\u0000\u0004$\u0001\u0000\u0000\u0000\u0006)\u0001"+
		"\u0000\u0000\u0000\b-\u0001\u0000\u0000\u0000\n=\u0001\u0000\u0000\u0000"+
		"\fD\u0001\u0000\u0000\u0000\u000eS\u0001\u0000\u0000\u0000\u0010p\u0001"+
		"\u0000\u0000\u0000\u0012\u0083\u0001\u0000\u0000\u0000\u0014\u0090\u0001"+
		"\u0000\u0000\u0000\u0016\u0018\u0003\u0002\u0001\u0000\u0017\u0016\u0001"+
		"\u0000\u0000\u0000\u0018\u0019\u0001\u0000\u0000\u0000\u0019\u0017\u0001"+
		"\u0000\u0000\u0000\u0019\u001a\u0001\u0000\u0000\u0000\u001a\u001b\u0001"+
		"\u0000\u0000\u0000\u001b\u001c\u0005\u0000\u0000\u0001\u001c\u0001\u0001"+
		"\u0000\u0000\u0000\u001d#\u0003\u0004\u0002\u0000\u001e#\u0003\u0006\u0003"+
		"\u0000\u001f#\u0003\b\u0004\u0000 #\u0003\f\u0006\u0000!#\u0003\u000e"+
		"\u0007\u0000\"\u001d\u0001\u0000\u0000\u0000\"\u001e\u0001\u0000\u0000"+
		"\u0000\"\u001f\u0001\u0000\u0000\u0000\" \u0001\u0000\u0000\u0000\"!\u0001"+
		"\u0000\u0000\u0000#\u0003\u0001\u0000\u0000\u0000$%\u0005\u000b\u0000"+
		"\u0000%&\u0005\u001b\u0000\u0000&\'\u0005\u0001\u0000\u0000\'(\u0003\u0010"+
		"\b\u0000(\u0005\u0001\u0000\u0000\u0000)*\u0005\u001b\u0000\u0000*+\u0005"+
		"\u0001\u0000\u0000+,\u0003\u0010\b\u0000,\u0007\u0001\u0000\u0000\u0000"+
		"-.\u0005\f\u0000\u0000./\u0005\u0002\u0000\u0000/0\u0003\u0010\b\u0000"+
		"01\u0005\u0003\u0000\u000013\u0005\u0004\u0000\u000024\u0003\u0002\u0001"+
		"\u000032\u0001\u0000\u0000\u000045\u0001\u0000\u0000\u000053\u0001\u0000"+
		"\u0000\u000056\u0001\u0000\u0000\u000068\u0001\u0000\u0000\u000079\u0003"+
		"\n\u0005\u000087\u0001\u0000\u0000\u000089\u0001\u0000\u0000\u00009:\u0001"+
		"\u0000\u0000\u0000:;\u0005\u0005\u0000\u0000;<\u0005\f\u0000\u0000<\t"+
		"\u0001\u0000\u0000\u0000=>\u0005\u0006\u0000\u0000>@\u0005\u0004\u0000"+
		"\u0000?A\u0003\u0002\u0001\u0000@?\u0001\u0000\u0000\u0000AB\u0001\u0000"+
		"\u0000\u0000B@\u0001\u0000\u0000\u0000BC\u0001\u0000\u0000\u0000C\u000b"+
		"\u0001\u0000\u0000\u0000DE\u0005\r\u0000\u0000EF\u0005\u0002\u0000\u0000"+
		"FG\u0003\u0010\b\u0000GH\u0005\u0003\u0000\u0000HJ\u0005\u0004\u0000\u0000"+
		"IK\u0003\u0002\u0001\u0000JI\u0001\u0000\u0000\u0000KL\u0001\u0000\u0000"+
		"\u0000LJ\u0001\u0000\u0000\u0000LM\u0001\u0000\u0000\u0000MN\u0001\u0000"+
		"\u0000\u0000NO\u0005\u0005\u0000\u0000OP\u0005\r\u0000\u0000P\r\u0001"+
		"\u0000\u0000\u0000QR\u0005\u001b\u0000\u0000RT\u0005\u0007\u0000\u0000"+
		"SQ\u0001\u0000\u0000\u0000ST\u0001\u0000\u0000\u0000TU\u0001\u0000\u0000"+
		"\u0000UV\u0005\u001b\u0000\u0000V_\u0005\u0002\u0000\u0000W\\\u0003\u0010"+
		"\b\u0000XY\u0005\b\u0000\u0000Y[\u0003\u0010\b\u0000ZX\u0001\u0000\u0000"+
		"\u0000[^\u0001\u0000\u0000\u0000\\Z\u0001\u0000\u0000\u0000\\]\u0001\u0000"+
		"\u0000\u0000]`\u0001\u0000\u0000\u0000^\\\u0001\u0000\u0000\u0000_W\u0001"+
		"\u0000\u0000\u0000_`\u0001\u0000\u0000\u0000`a\u0001\u0000\u0000\u0000"+
		"ab\u0005\u0003\u0000\u0000b\u000f\u0001\u0000\u0000\u0000cd\u0006\b\uffff"+
		"\uffff\u0000dq\u0005\u001b\u0000\u0000eq\u0005\u001c\u0000\u0000fq\u0005"+
		"\u001d\u0000\u0000gq\u0005\u000e\u0000\u0000hq\u0005\u001e\u0000\u0000"+
		"iq\u0003\u000e\u0007\u0000jk\u0005\u0002\u0000\u0000kl\u0003\u0010\b\u0000"+
		"lm\u0005\u0003\u0000\u0000mq\u0001\u0000\u0000\u0000nq\u0003\u0012\t\u0000"+
		"oq\u0003\u0014\n\u0000pc\u0001\u0000\u0000\u0000pe\u0001\u0000\u0000\u0000"+
		"pf\u0001\u0000\u0000\u0000pg\u0001\u0000\u0000\u0000ph\u0001\u0000\u0000"+
		"\u0000pi\u0001\u0000\u0000\u0000pj\u0001\u0000\u0000\u0000pn\u0001\u0000"+
		"\u0000\u0000po\u0001\u0000\u0000\u0000q\u0080\u0001\u0000\u0000\u0000"+
		"rs\n\r\u0000\u0000st\u0007\u0000\u0000\u0000t\u007f\u0003\u0010\b\u000e"+
		"uv\n\f\u0000\u0000vw\u0007\u0001\u0000\u0000w\u007f\u0003\u0010\b\rxy"+
		"\n\u000b\u0000\u0000yz\u0007\u0002\u0000\u0000z\u007f\u0003\u0010\b\f"+
		"{|\n\n\u0000\u0000|}\u0007\u0003\u0000\u0000}\u007f\u0003\u0010\b\u000b"+
		"~r\u0001\u0000\u0000\u0000~u\u0001\u0000\u0000\u0000~x\u0001\u0000\u0000"+
		"\u0000~{\u0001\u0000\u0000\u0000\u007f\u0082\u0001\u0000\u0000\u0000\u0080"+
		"~\u0001\u0000\u0000\u0000\u0080\u0081\u0001\u0000\u0000\u0000\u0081\u0011"+
		"\u0001\u0000\u0000\u0000\u0082\u0080\u0001\u0000\u0000\u0000\u0083\u008c"+
		"\u0005\t\u0000\u0000\u0084\u0089\u0003\u0010\b\u0000\u0085\u0086\u0005"+
		"\b\u0000\u0000\u0086\u0088\u0003\u0010\b\u0000\u0087\u0085\u0001\u0000"+
		"\u0000\u0000\u0088\u008b\u0001\u0000\u0000\u0000\u0089\u0087\u0001\u0000"+
		"\u0000\u0000\u0089\u008a\u0001\u0000\u0000\u0000\u008a\u008d\u0001\u0000"+
		"\u0000\u0000\u008b\u0089\u0001\u0000\u0000\u0000\u008c\u0084\u0001\u0000"+
		"\u0000\u0000\u008c\u008d\u0001\u0000\u0000\u0000\u008d\u008e\u0001\u0000"+
		"\u0000\u0000\u008e\u008f\u0005\n\u0000\u0000\u008f\u0013\u0001\u0000\u0000"+
		"\u0000\u0090\u0091\u0005\u001b\u0000\u0000\u0091\u0092\u0005\t\u0000\u0000"+
		"\u0092\u0093\u0003\u0010\b\u0000\u0093\u0094\u0005\n\u0000\u0000\u0094"+
		"\u0015\u0001\u0000\u0000\u0000\u000e\u0019\"58BLS\\_p~\u0080\u0089\u008c";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}