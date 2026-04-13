// Generated from c:/Users/kauan/OneDrive/Documentos/GitHub/backend-extensao/Jogo.Core/Linguagem.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link LinguagemParser}.
 */
public interface LinguagemListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link LinguagemParser#programa}.
	 * @param ctx the parse tree
	 */
	void enterPrograma(LinguagemParser.ProgramaContext ctx);
	/**
	 * Exit a parse tree produced by {@link LinguagemParser#programa}.
	 * @param ctx the parse tree
	 */
	void exitPrograma(LinguagemParser.ProgramaContext ctx);
	/**
	 * Enter a parse tree produced by {@link LinguagemParser#comando}.
	 * @param ctx the parse tree
	 */
	void enterComando(LinguagemParser.ComandoContext ctx);
	/**
	 * Exit a parse tree produced by {@link LinguagemParser#comando}.
	 * @param ctx the parse tree
	 */
	void exitComando(LinguagemParser.ComandoContext ctx);
	/**
	 * Enter a parse tree produced by {@link LinguagemParser#declaracaoVariavel}.
	 * @param ctx the parse tree
	 */
	void enterDeclaracaoVariavel(LinguagemParser.DeclaracaoVariavelContext ctx);
	/**
	 * Exit a parse tree produced by {@link LinguagemParser#declaracaoVariavel}.
	 * @param ctx the parse tree
	 */
	void exitDeclaracaoVariavel(LinguagemParser.DeclaracaoVariavelContext ctx);
	/**
	 * Enter a parse tree produced by {@link LinguagemParser#atribuicao}.
	 * @param ctx the parse tree
	 */
	void enterAtribuicao(LinguagemParser.AtribuicaoContext ctx);
	/**
	 * Exit a parse tree produced by {@link LinguagemParser#atribuicao}.
	 * @param ctx the parse tree
	 */
	void exitAtribuicao(LinguagemParser.AtribuicaoContext ctx);
	/**
	 * Enter a parse tree produced by {@link LinguagemParser#estruturaSe}.
	 * @param ctx the parse tree
	 */
	void enterEstruturaSe(LinguagemParser.EstruturaSeContext ctx);
	/**
	 * Exit a parse tree produced by {@link LinguagemParser#estruturaSe}.
	 * @param ctx the parse tree
	 */
	void exitEstruturaSe(LinguagemParser.EstruturaSeContext ctx);
	/**
	 * Enter a parse tree produced by {@link LinguagemParser#estruturaSenao}.
	 * @param ctx the parse tree
	 */
	void enterEstruturaSenao(LinguagemParser.EstruturaSenaoContext ctx);
	/**
	 * Exit a parse tree produced by {@link LinguagemParser#estruturaSenao}.
	 * @param ctx the parse tree
	 */
	void exitEstruturaSenao(LinguagemParser.EstruturaSenaoContext ctx);
	/**
	 * Enter a parse tree produced by {@link LinguagemParser#estruturaEnquanto}.
	 * @param ctx the parse tree
	 */
	void enterEstruturaEnquanto(LinguagemParser.EstruturaEnquantoContext ctx);
	/**
	 * Exit a parse tree produced by {@link LinguagemParser#estruturaEnquanto}.
	 * @param ctx the parse tree
	 */
	void exitEstruturaEnquanto(LinguagemParser.EstruturaEnquantoContext ctx);
	/**
	 * Enter a parse tree produced by {@link LinguagemParser#chamadaFuncao}.
	 * @param ctx the parse tree
	 */
	void enterChamadaFuncao(LinguagemParser.ChamadaFuncaoContext ctx);
	/**
	 * Exit a parse tree produced by {@link LinguagemParser#chamadaFuncao}.
	 * @param ctx the parse tree
	 */
	void exitChamadaFuncao(LinguagemParser.ChamadaFuncaoContext ctx);
	/**
	 * Enter a parse tree produced by {@link LinguagemParser#expressao}.
	 * @param ctx the parse tree
	 */
	void enterExpressao(LinguagemParser.ExpressaoContext ctx);
	/**
	 * Exit a parse tree produced by {@link LinguagemParser#expressao}.
	 * @param ctx the parse tree
	 */
	void exitExpressao(LinguagemParser.ExpressaoContext ctx);
	/**
	 * Enter a parse tree produced by {@link LinguagemParser#lista}.
	 * @param ctx the parse tree
	 */
	void enterLista(LinguagemParser.ListaContext ctx);
	/**
	 * Exit a parse tree produced by {@link LinguagemParser#lista}.
	 * @param ctx the parse tree
	 */
	void exitLista(LinguagemParser.ListaContext ctx);
	/**
	 * Enter a parse tree produced by {@link LinguagemParser#acessoLista}.
	 * @param ctx the parse tree
	 */
	void enterAcessoLista(LinguagemParser.AcessoListaContext ctx);
	/**
	 * Exit a parse tree produced by {@link LinguagemParser#acessoLista}.
	 * @param ctx the parse tree
	 */
	void exitAcessoLista(LinguagemParser.AcessoListaContext ctx);
}