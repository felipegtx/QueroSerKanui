Kanui
=====

Solução para o problema '[digital display](https://github.com/Kanui/QueroSerKanui/tree/master/testes/digital-display)'.

Versão compilada
------
Disponível em formato [7zip](http://www.7-zip.org/) [*aqui*](https://github.com/felipegtx/Kanui/raw/master/Release.7z).

Utilizando a solução
------
A compilação resulta em um arquivo executável de nome *Kanui.exe* o qual pode ser utilizado de duas formas distintas, a saber:

**Treinamento**
O programa interpretará o parâmetro como sendo um arquivo de texto que será utilizado para cálculo do hash de identificação dos dígitos. Como resultado o programa atualizará o arquivo de indices de caracteres que o acompanha (map.kanui) e passará a considerar este novo formato em futuras execuções.

*IMPORTANTE:* Colisões e erros de cálculo serão identificados no output como '/!\\erro de formato/!\' e NÃO atualizarão o arquivo de indexação.

*EXEMPLO:* Kanui.exe "t>C:\\MeuArquivo.txt"

**Identificação**
Com base no arquivo de indices, o programa tentará identificar os caracteres existentes no arquivo seguindo o conhecimento obtido em treinamentos prévios. 

Erros de parse serão exibidos com a mensagem: '/!\\erro de formato/!\'

*EXEMPLO:* Kanui.exe "i>C:\\MeuArquivo.txt"

Reconhecimento dos dígitos
------

Por meio da [seguinte função de Hash](https://github.com/felipegtx/Kanui/blob/master/Projeto/Kanui/Parsers/DataParserResult.cs#L186), é possível extrair um identificador único para cada grupo de caracteres que representam um dado dígito no display:

`acumulator += (((y ^ d) + (xRef ^ d)) / 3) + (d * y);`

Onde *d* é o resultado da função **GetHashCode** do char em cada uma das posições do display.

Tecnologia utilizada
------

* .Net Framework 4.5 (C#)
* .MSTests



