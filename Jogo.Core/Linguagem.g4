grammar Linguagem;

// ==========================================
// PARSER (Regras Sintáticas - Minúsculas)
// ==========================================

programa: comando+ EOF;

// Os tipos de comandos que o jogador pode digitar
comando: declaracaoVariavel
       | atribuicao
       | estruturaSe
       | estruturaEnquanto
       | declaracaoFuncao
       | comandoRetorno
       | chamadaFuncao;

// Declaração de variável 
declaracaoVariavel: TIPO ID '=' expressao;

// Atribuição de valor 
atribuicao: ID '=' expressao;

// Controle de fluxo: enquanto(condição):
estruturaEnquanto: ENQUANTO '(' expressao ')' ':' comando+ 'fim' 'enquanto';

// O bloco 'senão'
estruturaSenao: 'senao' ':' comando+;

// O bloco 'senão se'
estruturaSenaoSe: 'senao' 'se' '(' expressao ')' ':' comando+;

// Controle de fluxo principal
estruturaSe: SE '(' expressao ')' ':' comando+ estruturaSenaoSe* estruturaSenao? 'fim' 'se';

parametro: TIPO ID;

// Exemplo: int curar(int forca, string alvo): ... fim funcao
declaracaoFuncao: TIPO ID '(' (parametro (',' parametro)*)? ')' ':' comando+ 'fim' 'funcao';

// Regra de retorno
comandoRetorno: 'retorna' expressao?;

// Permite chamadas simples 'funcao()' ou membros 'objeto.funcao()'
chamadaFuncao: (objeto=ID '.')? funcao=ID '(' (expressao (',' expressao)*)? ')';

// Expressões matemáticas, lógicas e estruturas de dados
expressao: expressao (MULT | DIV | MOD) expressao
         | expressao (SOMA | SUB) expressao
         | expressao ( MAIOR | MENOR | MAIOR_IGUAL | MENOR_IGUAL | IGUAL) expressao
         | expressao (E | OU) expressao
         | ID
         | NUMERO_INT
         | NUMERO_FLOAT
         | BOOLEANO
         | STRING_LIT
         | chamadaFuncao
         | '(' expressao ')'
         | lista
         | acessoLista
         | acessoAtributo
         | NAO expressao;

// Listas e acesso por índice (ex: vetor = [1, 2, 3] e vetor[0])
lista: '[' (expressao (',' expressao)*)? ']';
acessoLista: ID '[' expressao ']';


// ==========================================
// LEXER (Vocabulário / Tokens - MAIÚSCULAS)
// ==========================================

// Tipos 
TIPO: 'vazio' | 'int' | 'float' | 'bool' | 'string'| 'Inimigo' | 'Arena' | 'Ataque' | 'Direcao';

// Palavras reservadas
SE: 'se';
ENQUANTO: 'enquanto';

// Valores Lógicos
BOOLEANO: 'Verdadeiro' | 'Falso';

// Operadores
SOMA: '+';
SUB: '-';
MULT: '*';
DIV: '/';
MOD: '%';
E: 'e';
OU: 'ou';
IGUAL: '==';
MENOR: '<';
MAIOR: '>';
MAIOR_IGUAL: '>=';
MENOR_IGUAL: '<=';
NAO: '!';

// Identificadores (Nomes de variáveis e funções)
ID: [a-zA-Z_][a-zA-Z0-9_]*;

// Tipos numéricos e texto
NUMERO_INT: [0-9]+;
NUMERO_FLOAT: [0-9]+ '.' [0-9]+;
STRING_LIT: '"' ~["]* '"';

// Ignorar espaços, tabs e quebras de linha na hora de ler o código
WS: [ \t\r\n]+ -> skip;

acessoAtributo: ID '.' ID;
// Sistema de comentário: # + *qualquer coisa* -> Ignorado (até o fim da linha)
COMENTARIO: '#' ~[\r\n]* -> skip;
