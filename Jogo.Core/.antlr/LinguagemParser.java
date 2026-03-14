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
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, TIPO=8, SE=9, 
		ENQUANTO=10, BOOLEANO=11, SOMA=12, SUB=13, MULT=14, DIV=15, MOD=16, E=17, 
		OU=18, IGUAL=19, MAIOR_IGUAL=20, MENOR_IGUAL=21, ID=22, NUMERO_INT=23, 
		NUMERO_FLOAT=24, STRING_LIT=25, WS=26;
	public static final int
		RULE_programa = 0, RULE_comando = 1, RULE_declaracaoVariavel = 2, RULE_atribuicao = 3, 
		RULE_estruturaSe = 4, RULE_estruturaEnquanto = 5, RULE_chamadaFuncao = 6, 
		RULE_expressao = 7, RULE_lista = 8, RULE_acessoLista = 9;
	private static String[] makeRuleNames() {
		return new String[] {
			"programa", "comando", "declaracaoVariavel", "atribuicao", "estruturaSe", 
			"estruturaEnquanto", "chamadaFuncao", "expressao", "lista", "acessoLista"
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).enterPrograma(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).exitPrograma(this);
		}
	}

	public final ProgramaContext programa() throws RecognitionException {
		ProgramaContext _localctx = new ProgramaContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_programa);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(21); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(20);
				comando();
				}
				}
				setState(23); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & 4196096L) != 0) );
			setState(25);
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).enterComando(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).exitComando(this);
		}
	}

	public final ComandoContext comando() throws RecognitionException {
		ComandoContext _localctx = new ComandoContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_comando);
		try {
			setState(32);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,1,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(27);
				declaracaoVariavel();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(28);
				atribuicao();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(29);
				estruturaSe();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(30);
				estruturaEnquanto();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(31);
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).enterDeclaracaoVariavel(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).exitDeclaracaoVariavel(this);
		}
	}

	public final DeclaracaoVariavelContext declaracaoVariavel() throws RecognitionException {
		DeclaracaoVariavelContext _localctx = new DeclaracaoVariavelContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_declaracaoVariavel);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(34);
			match(TIPO);
			setState(35);
			match(ID);
			setState(36);
			match(T__0);
			setState(37);
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).enterAtribuicao(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).exitAtribuicao(this);
		}
	}

	public final AtribuicaoContext atribuicao() throws RecognitionException {
		AtribuicaoContext _localctx = new AtribuicaoContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_atribuicao);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(39);
			match(ID);
			setState(40);
			match(T__0);
			setState(41);
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
		public EstruturaSeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_estruturaSe; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).enterEstruturaSe(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).exitEstruturaSe(this);
		}
	}

	public final EstruturaSeContext estruturaSe() throws RecognitionException {
		EstruturaSeContext _localctx = new EstruturaSeContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_estruturaSe);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(43);
			match(SE);
			setState(44);
			match(T__1);
			setState(45);
			expressao(0);
			setState(46);
			match(T__2);
			setState(47);
			match(T__3);
			setState(49); 
			_errHandler.sync(this);
			_alt = 1;
			do {
				switch (_alt) {
				case 1:
					{
					{
					setState(48);
					comando();
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(51); 
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,2,_ctx);
			} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
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
		public TerminalNode ENQUANTO() { return getToken(LinguagemParser.ENQUANTO, 0); }
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).enterEstruturaEnquanto(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).exitEstruturaEnquanto(this);
		}
	}

	public final EstruturaEnquantoContext estruturaEnquanto() throws RecognitionException {
		EstruturaEnquantoContext _localctx = new EstruturaEnquantoContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_estruturaEnquanto);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(53);
			match(ENQUANTO);
			setState(54);
			match(T__1);
			setState(55);
			expressao(0);
			setState(56);
			match(T__2);
			setState(57);
			match(T__3);
			setState(59); 
			_errHandler.sync(this);
			_alt = 1;
			do {
				switch (_alt) {
				case 1:
					{
					{
					setState(58);
					comando();
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(61); 
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,3,_ctx);
			} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
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
		public TerminalNode ID() { return getToken(LinguagemParser.ID, 0); }
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).enterChamadaFuncao(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).exitChamadaFuncao(this);
		}
	}

	public final ChamadaFuncaoContext chamadaFuncao() throws RecognitionException {
		ChamadaFuncaoContext _localctx = new ChamadaFuncaoContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_chamadaFuncao);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(63);
			match(ID);
			setState(64);
			match(T__1);
			setState(73);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 62916676L) != 0)) {
				{
				setState(65);
				expressao(0);
				setState(70);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__4) {
					{
					{
					setState(66);
					match(T__4);
					setState(67);
					expressao(0);
					}
					}
					setState(72);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(75);
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
		public TerminalNode MAIOR_IGUAL() { return getToken(LinguagemParser.MAIOR_IGUAL, 0); }
		public TerminalNode MENOR_IGUAL() { return getToken(LinguagemParser.MENOR_IGUAL, 0); }
		public TerminalNode IGUAL() { return getToken(LinguagemParser.IGUAL, 0); }
		public TerminalNode E() { return getToken(LinguagemParser.E, 0); }
		public TerminalNode OU() { return getToken(LinguagemParser.OU, 0); }
		public ExpressaoContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expressao; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).enterExpressao(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).exitExpressao(this);
		}
	}

	public final ExpressaoContext expressao() throws RecognitionException {
		return expressao(0);
	}

	private ExpressaoContext expressao(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExpressaoContext _localctx = new ExpressaoContext(_ctx, _parentState);
		ExpressaoContext _prevctx = _localctx;
		int _startState = 14;
		enterRecursionRule(_localctx, 14, RULE_expressao, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(90);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,6,_ctx) ) {
			case 1:
				{
				setState(78);
				match(ID);
				}
				break;
			case 2:
				{
				setState(79);
				match(NUMERO_INT);
				}
				break;
			case 3:
				{
				setState(80);
				match(NUMERO_FLOAT);
				}
				break;
			case 4:
				{
				setState(81);
				match(BOOLEANO);
				}
				break;
			case 5:
				{
				setState(82);
				match(STRING_LIT);
				}
				break;
			case 6:
				{
				setState(83);
				chamadaFuncao();
				}
				break;
			case 7:
				{
				setState(84);
				match(T__1);
				setState(85);
				expressao(0);
				setState(86);
				match(T__2);
				}
				break;
			case 8:
				{
				setState(88);
				lista();
				}
				break;
			case 9:
				{
				setState(89);
				acessoLista();
				}
				break;
			}
			_ctx.stop = _input.LT(-1);
			setState(106);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,8,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(104);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,7,_ctx) ) {
					case 1:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(92);
						if (!(precpred(_ctx, 13))) throw new FailedPredicateException(this, "precpred(_ctx, 13)");
						setState(93);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 114688L) != 0)) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(94);
						expressao(14);
						}
						break;
					case 2:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(95);
						if (!(precpred(_ctx, 12))) throw new FailedPredicateException(this, "precpred(_ctx, 12)");
						setState(96);
						_la = _input.LA(1);
						if ( !(_la==SOMA || _la==SUB) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(97);
						expressao(13);
						}
						break;
					case 3:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(98);
						if (!(precpred(_ctx, 11))) throw new FailedPredicateException(this, "precpred(_ctx, 11)");
						setState(99);
						_la = _input.LA(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 3670016L) != 0)) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(100);
						expressao(12);
						}
						break;
					case 4:
						{
						_localctx = new ExpressaoContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expressao);
						setState(101);
						if (!(precpred(_ctx, 10))) throw new FailedPredicateException(this, "precpred(_ctx, 10)");
						setState(102);
						_la = _input.LA(1);
						if ( !(_la==E || _la==OU) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(103);
						expressao(11);
						}
						break;
					}
					} 
				}
				setState(108);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,8,_ctx);
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).enterLista(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).exitLista(this);
		}
	}

	public final ListaContext lista() throws RecognitionException {
		ListaContext _localctx = new ListaContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_lista);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(109);
			match(T__5);
			setState(118);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 62916676L) != 0)) {
				{
				setState(110);
				expressao(0);
				setState(115);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__4) {
					{
					{
					setState(111);
					match(T__4);
					setState(112);
					expressao(0);
					}
					}
					setState(117);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
			}

			setState(120);
			match(T__6);
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).enterAcessoLista(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof LinguagemListener ) ((LinguagemListener)listener).exitAcessoLista(this);
		}
	}

	public final AcessoListaContext acessoLista() throws RecognitionException {
		AcessoListaContext _localctx = new AcessoListaContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_acessoLista);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(122);
			match(ID);
			setState(123);
			match(T__5);
			setState(124);
			expressao(0);
			setState(125);
			match(T__6);
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
		case 7:
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
		"\u0004\u0001\u001a\u0080\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001"+
		"\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004\u0007\u0004"+
		"\u0002\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007\u0007\u0007"+
		"\u0002\b\u0007\b\u0002\t\u0007\t\u0001\u0000\u0004\u0000\u0016\b\u0000"+
		"\u000b\u0000\f\u0000\u0017\u0001\u0000\u0001\u0000\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0003\u0001!\b\u0001\u0001\u0002"+
		"\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0002\u0001\u0003\u0001\u0003"+
		"\u0001\u0003\u0001\u0003\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0004"+
		"\u0001\u0004\u0001\u0004\u0004\u00042\b\u0004\u000b\u0004\f\u00043\u0001"+
		"\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0004"+
		"\u0005<\b\u0005\u000b\u0005\f\u0005=\u0001\u0006\u0001\u0006\u0001\u0006"+
		"\u0001\u0006\u0001\u0006\u0005\u0006E\b\u0006\n\u0006\f\u0006H\t\u0006"+
		"\u0003\u0006J\b\u0006\u0001\u0006\u0001\u0006\u0001\u0007\u0001\u0007"+
		"\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007"+
		"\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0003\u0007"+
		"[\b\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007"+
		"\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007"+
		"\u0001\u0007\u0005\u0007i\b\u0007\n\u0007\f\u0007l\t\u0007\u0001\b\u0001"+
		"\b\u0001\b\u0001\b\u0005\br\b\b\n\b\f\bu\t\b\u0003\bw\b\b\u0001\b\u0001"+
		"\b\u0001\t\u0001\t\u0001\t\u0001\t\u0001\t\u0001\t\u0000\u0001\u000e\n"+
		"\u0000\u0002\u0004\u0006\b\n\f\u000e\u0010\u0012\u0000\u0004\u0001\u0000"+
		"\u000e\u0010\u0001\u0000\f\r\u0001\u0000\u0013\u0015\u0001\u0000\u0011"+
		"\u0012\u008c\u0000\u0015\u0001\u0000\u0000\u0000\u0002 \u0001\u0000\u0000"+
		"\u0000\u0004\"\u0001\u0000\u0000\u0000\u0006\'\u0001\u0000\u0000\u0000"+
		"\b+\u0001\u0000\u0000\u0000\n5\u0001\u0000\u0000\u0000\f?\u0001\u0000"+
		"\u0000\u0000\u000eZ\u0001\u0000\u0000\u0000\u0010m\u0001\u0000\u0000\u0000"+
		"\u0012z\u0001\u0000\u0000\u0000\u0014\u0016\u0003\u0002\u0001\u0000\u0015"+
		"\u0014\u0001\u0000\u0000\u0000\u0016\u0017\u0001\u0000\u0000\u0000\u0017"+
		"\u0015\u0001\u0000\u0000\u0000\u0017\u0018\u0001\u0000\u0000\u0000\u0018"+
		"\u0019\u0001\u0000\u0000\u0000\u0019\u001a\u0005\u0000\u0000\u0001\u001a"+
		"\u0001\u0001\u0000\u0000\u0000\u001b!\u0003\u0004\u0002\u0000\u001c!\u0003"+
		"\u0006\u0003\u0000\u001d!\u0003\b\u0004\u0000\u001e!\u0003\n\u0005\u0000"+
		"\u001f!\u0003\f\u0006\u0000 \u001b\u0001\u0000\u0000\u0000 \u001c\u0001"+
		"\u0000\u0000\u0000 \u001d\u0001\u0000\u0000\u0000 \u001e\u0001\u0000\u0000"+
		"\u0000 \u001f\u0001\u0000\u0000\u0000!\u0003\u0001\u0000\u0000\u0000\""+
		"#\u0005\b\u0000\u0000#$\u0005\u0016\u0000\u0000$%\u0005\u0001\u0000\u0000"+
		"%&\u0003\u000e\u0007\u0000&\u0005\u0001\u0000\u0000\u0000\'(\u0005\u0016"+
		"\u0000\u0000()\u0005\u0001\u0000\u0000)*\u0003\u000e\u0007\u0000*\u0007"+
		"\u0001\u0000\u0000\u0000+,\u0005\t\u0000\u0000,-\u0005\u0002\u0000\u0000"+
		"-.\u0003\u000e\u0007\u0000./\u0005\u0003\u0000\u0000/1\u0005\u0004\u0000"+
		"\u000002\u0003\u0002\u0001\u000010\u0001\u0000\u0000\u000023\u0001\u0000"+
		"\u0000\u000031\u0001\u0000\u0000\u000034\u0001\u0000\u0000\u00004\t\u0001"+
		"\u0000\u0000\u000056\u0005\n\u0000\u000067\u0005\u0002\u0000\u000078\u0003"+
		"\u000e\u0007\u000089\u0005\u0003\u0000\u00009;\u0005\u0004\u0000\u0000"+
		":<\u0003\u0002\u0001\u0000;:\u0001\u0000\u0000\u0000<=\u0001\u0000\u0000"+
		"\u0000=;\u0001\u0000\u0000\u0000=>\u0001\u0000\u0000\u0000>\u000b\u0001"+
		"\u0000\u0000\u0000?@\u0005\u0016\u0000\u0000@I\u0005\u0002\u0000\u0000"+
		"AF\u0003\u000e\u0007\u0000BC\u0005\u0005\u0000\u0000CE\u0003\u000e\u0007"+
		"\u0000DB\u0001\u0000\u0000\u0000EH\u0001\u0000\u0000\u0000FD\u0001\u0000"+
		"\u0000\u0000FG\u0001\u0000\u0000\u0000GJ\u0001\u0000\u0000\u0000HF\u0001"+
		"\u0000\u0000\u0000IA\u0001\u0000\u0000\u0000IJ\u0001\u0000\u0000\u0000"+
		"JK\u0001\u0000\u0000\u0000KL\u0005\u0003\u0000\u0000L\r\u0001\u0000\u0000"+
		"\u0000MN\u0006\u0007\uffff\uffff\u0000N[\u0005\u0016\u0000\u0000O[\u0005"+
		"\u0017\u0000\u0000P[\u0005\u0018\u0000\u0000Q[\u0005\u000b\u0000\u0000"+
		"R[\u0005\u0019\u0000\u0000S[\u0003\f\u0006\u0000TU\u0005\u0002\u0000\u0000"+
		"UV\u0003\u000e\u0007\u0000VW\u0005\u0003\u0000\u0000W[\u0001\u0000\u0000"+
		"\u0000X[\u0003\u0010\b\u0000Y[\u0003\u0012\t\u0000ZM\u0001\u0000\u0000"+
		"\u0000ZO\u0001\u0000\u0000\u0000ZP\u0001\u0000\u0000\u0000ZQ\u0001\u0000"+
		"\u0000\u0000ZR\u0001\u0000\u0000\u0000ZS\u0001\u0000\u0000\u0000ZT\u0001"+
		"\u0000\u0000\u0000ZX\u0001\u0000\u0000\u0000ZY\u0001\u0000\u0000\u0000"+
		"[j\u0001\u0000\u0000\u0000\\]\n\r\u0000\u0000]^\u0007\u0000\u0000\u0000"+
		"^i\u0003\u000e\u0007\u000e_`\n\f\u0000\u0000`a\u0007\u0001\u0000\u0000"+
		"ai\u0003\u000e\u0007\rbc\n\u000b\u0000\u0000cd\u0007\u0002\u0000\u0000"+
		"di\u0003\u000e\u0007\fef\n\n\u0000\u0000fg\u0007\u0003\u0000\u0000gi\u0003"+
		"\u000e\u0007\u000bh\\\u0001\u0000\u0000\u0000h_\u0001\u0000\u0000\u0000"+
		"hb\u0001\u0000\u0000\u0000he\u0001\u0000\u0000\u0000il\u0001\u0000\u0000"+
		"\u0000jh\u0001\u0000\u0000\u0000jk\u0001\u0000\u0000\u0000k\u000f\u0001"+
		"\u0000\u0000\u0000lj\u0001\u0000\u0000\u0000mv\u0005\u0006\u0000\u0000"+
		"ns\u0003\u000e\u0007\u0000op\u0005\u0005\u0000\u0000pr\u0003\u000e\u0007"+
		"\u0000qo\u0001\u0000\u0000\u0000ru\u0001\u0000\u0000\u0000sq\u0001\u0000"+
		"\u0000\u0000st\u0001\u0000\u0000\u0000tw\u0001\u0000\u0000\u0000us\u0001"+
		"\u0000\u0000\u0000vn\u0001\u0000\u0000\u0000vw\u0001\u0000\u0000\u0000"+
		"wx\u0001\u0000\u0000\u0000xy\u0005\u0007\u0000\u0000y\u0011\u0001\u0000"+
		"\u0000\u0000z{\u0005\u0016\u0000\u0000{|\u0005\u0006\u0000\u0000|}\u0003"+
		"\u000e\u0007\u0000}~\u0005\u0007\u0000\u0000~\u0013\u0001\u0000\u0000"+
		"\u0000\u000b\u0017 3=FIZhjsv";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}