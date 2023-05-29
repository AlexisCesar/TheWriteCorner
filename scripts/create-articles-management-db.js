use Articles_Management

db.createCollection("Articles", {
    validator: {
        $jsonSchema: {
          bsonType: "object",
          required: ["_id", "Title", "Content", "Authors", "PublicationDate", "Keywords", "LastUpdate", "ReadingEstimatedTime", "LikeCount", "Comments", "Abstract"],
          properties: {
            _id: {
              bsonType: "objectId",
              description: "Insert a valid id.",
            },
            Title: {
              bsonType: "string",
              description: "Insert a valid title.",
            },
            Content: {
              bsonType: "string",
              description: "Insert a valid content text.",
            },
            Authors: {
              bsonType: "array",
              description: "Insert a valid author array.",
            },
            PublicationDate: {
              bsonType: "date",
              description: "Insert a valid publication date.",
            },
            Keywords: {
              bsonType: "array",
              description: "Insert a valid keyword array.",
            },
            LastUpdate: {
              bsonType: "date",
              description: "Insert a valid last update date.",
            },
            ReadingEstimatedTime: {
                bsonType: "int",
                minimum: 1,
                description: "Insert a valid reading time."
            },
            LikeCount: {
                bsonType: "int",
                minimum: 0,
                description: "Insert a valid like count."
            },
            Comments: {
                bsonType: "array",
                items: {
                    bsonType: "object",
                    required: ["Author", "Text"],
                    properties: {
                    Author: {
                      bsonType: "string"
                    },
                    Text: {
                      bsonType: "string"
                    }
                  },
                  additionalProperties: false
                },
                description: "Insert a valid comment array."
            },
            Abstract: {
                bsonType: "string",
                description: "Insert a valid abstract text."
            }
          },
          additionalProperties: false
        }
    }
});

db.Articles.insertMany([{
    "Title": "A IMPORTÂNCIA DO TEOREMA FUNDAMENTAL DA ARITMÉTICA NA CRIPTOGRAFIA DE CHAVE PÚBLICA",
    "Abstract": "O presente trabalho teve como objetivo apresentar a importância da aplicabilidadedo teorema fundamental da aritmética na criptografia de chave pública, no qual seupapel é imprescindível na segurança da informação devido a dificuldade em decompornúmeroscompostosem fatores primos. Para isso,o estudo baseou-se emrevisões bibliográficas com demonstrações matemáticas do postuladoe da infinitude dos números primos, bem como um algoritmo desenvolvido pelos autores que realiza a fatoração de doisnúmeros distintos em suas quantidades de bits, evidenciandoassim,que tal operação computacional é onerosae não determinística. Nesse sentido, a pesquisa apresentaempiricamenteatravés de experimentos e demonstrações,o quão laborioso seria atentativa de obtenção inapropriada dos dadoscriptografados utilizando o algoritmo de chave assimétrica, salientando que, a computação une-se àmatemática para garantir a impraticabilidade nadecriptação de uma chave pública, garantindo dessa forma, a segurança e a integridade da informaçãono mundo digital.",
    "Authors": ["Bruna Gonçalves"],
    "Comments": [{"Author": "User123", "Text": "I like it!"}],
    "Content": "1 INTRODUÇÃO - A criptografia é o estudo de técnicas matemáticas e computacionais relacionadas com a segurança da informação que visam ocultar dados (SANT’ ANA JÚNIOR, 2013). Desde que o ser humano começou a comunicar-se percebeu que nem sempre era de sua vontade que terceiros acessem as informações transmitidas. Para esconder suas comunicações surgiram técnicas de ocultação para tornar inteligíveis as mensagens repassadas para outros de forma que somente o remetente e o destinatário seriam capazes de saber o conteúdo original. As técnicas de ocultação destas informações desenvolveram-se de forma a utilizar conceitos da matemática, inicialmente criados sem finalidade prática mas que encaixam-se perfeitamente à segurança da informação (SANT’ ANA JÚNIOR, 2013).A criptografia de chave públicabaseia-se no teorema fundamental da aritmética, o mesmo postula que todo número composto pode ser representado em seus fatores primos. O maior desafio, contudo, está em fatorar números grandes, pois não existe um padrão na sequência dos números primos (SOUZA, 2018). Baseado nisso, o objetivo deste trabalho é evidenciar a significância obtida pelo uso prático do teorema fundamental da aritmética na criptografia de chave pública, através de revisões bibliográficas, apresentação de deduções matemáticas, bem como um algoritmo relativamente performáticoelaborado pelos autores com intuito de simular a decomposição numérica de doisnúmeros diferentes em quantidades de bits, expondo assim, de maneira empírica, a complexidade e inviabilidade computacional da fatoração prima de um número composto. Espera-se, então, salientar a importância da matemática em fusão com a computação, de modo que garanta a segurança e integridade dos dados",
    "Keywords": ["Teorema", "Algoritmo", "Aritmética", "Criptografia", "Chave-pública", "Fatoração."],
    "LastUpdate": new ISODate("2020-12-18T00:00:00Z"),
    "LikeCount": 200,
    "PublicationDate": new ISODate("2020-12-18T00:00:00Z"),
    "ReadingEstimatedTime": 10
}]);