using System;
using System.Collections.Generic;

class Grafo<T>
{
    private Dictionary<T, Dictionary<T, int>> grafo = new Dictionary<T, Dictionary<T, int>>();

    public void AdicionaVertice(T vertice)
    {
        grafo[vertice] = new Dictionary<T, int>();
    }

    public void AdicionaAresta(int peso, T vertice1, T vertice2)
    {
        grafo[vertice1][vertice2] = peso;
        grafo[vertice2][vertice1] = peso;
    }

    public Dictionary<T, int> ObterVizinhos(T vertice)
    {
        return grafo[vertice];
    }

    public HashSet<T> ObterVertices()
    {
        return new HashSet<T>(grafo.Keys);
    }
}

class AppGrafo
{
    static void Main()
    {
        Grafo<string> grafo = new Grafo<string>();

        grafo.AdicionaVertice("A");
        grafo.AdicionaVertice("B");
        grafo.AdicionaVertice("C");
        grafo.AdicionaVertice("D");
        grafo.AdicionaVertice("E");
        grafo.AdicionaVertice("F");
        grafo.AdicionaVertice("G");
        grafo.AdicionaVertice("H");
        grafo.AdicionaVertice("I");
        grafo.AdicionaVertice("J");
        grafo.AdicionaVertice("K");
        grafo.AdicionaVertice("L");
        grafo.AdicionaVertice("M");
        grafo.AdicionaVertice("N");
        grafo.AdicionaVertice("O");
        grafo.AdicionaVertice("P");
        grafo.AdicionaVertice("Q");
        grafo.AdicionaVertice("R");
        grafo.AdicionaVertice("S");
        grafo.AdicionaVertice("T");
        grafo.AdicionaVertice("U");
        grafo.AdicionaVertice("V");
        grafo.AdicionaVertice("X");

        grafo.AdicionaAresta(300, "A", "B");
        grafo.AdicionaAresta(47, "B", "C");
        grafo.AdicionaAresta(62, "C", "D");
        grafo.AdicionaAresta(8, "D", "E");
        grafo.AdicionaAresta(13, "E", "F");
        grafo.AdicionaAresta(230, "E", "G");
        grafo.AdicionaAresta(141, "C", "H");
        grafo.AdicionaAresta(138, "H", "I");
        grafo.AdicionaAresta(153, "I", "J");
        grafo.AdicionaAresta(512, "J", "K");
        grafo.AdicionaAresta(135, "K", "L");
        grafo.AdicionaAresta(50, "L", "M");
        grafo.AdicionaAresta(187, "L", "N");
        grafo.AdicionaAresta(108, "N", "O");
        grafo.AdicionaAresta(82, "O", "P");
        grafo.AdicionaAresta(215, "P", "Q");
        grafo.AdicionaAresta(97, "Q", "R");
        grafo.AdicionaAresta(243, "R", "T");
        grafo.AdicionaAresta(33, "R", "S");
        grafo.AdicionaAresta(207, "S", "T");
        grafo.AdicionaAresta(38, "S", "V");
        grafo.AdicionaAresta(22, "T", "U");
        grafo.AdicionaAresta(210, "V", "U");
        grafo.AdicionaAresta(370, "V", "A");
        grafo.AdicionaAresta(107, "U", "X");
        grafo.AdicionaAresta(317, "X", "A");

        Console.Write("Digite o ponto de partida: ");
        string partida = Console.ReadLine();

        Console.Write("Digite o ponto de chegada: ");
        string chegada = Console.ReadLine();

        Dijkstra<string> dijkstra = new Dijkstra<string>(grafo);
        dijkstra.EncontrarMenorCaminho(partida);

        List<List<string>> todosOsCaminhos = dijkstra.GetTodosOsCaminhos(chegada);
        int distancia = dijkstra.GetDistancia(chegada);

        if (todosOsCaminhos != null && todosOsCaminhos.Count > 0)
        {
            Console.WriteLine($"Distância mais curta: {distancia} metros");

            for (int i = 0; i < Math.Min(2, todosOsCaminhos.Count); i++)
            {
                List<string> caminho = todosOsCaminhos[i];
                Console.WriteLine($"Caminho {i + 1}: {string.Join(" -> ", caminho)}");
            }
        } else
        {
            Console.WriteLine("Não há caminho entre os pontos de partida e chegada.");
        }
    }
}

class Dijkstra<T>
{
    private Grafo<T> grafo;
    private Dictionary<T, int> distancia;
    private Dictionary<T, T> anterior;
    private Dictionary<T, List<List<T>>> todosOsCaminhos;

    public Dijkstra(Grafo<T> grafo)
    {
        this.grafo = grafo;
        this.distancia = new Dictionary<T, int>();
        this.anterior = new Dictionary<T, T>();
        this.todosOsCaminhos = new Dictionary<T, List<List<T>>>();
    }

    public void EncontrarMenorCaminho(T origem)
    {
        PriorityQueue<T> filaPrioridade = new PriorityQueue<T>((v1, v2) => distancia[v1].CompareTo(distancia[v2]));

        foreach (T vertice in grafo.ObterVertices())
        {
            distancia[vertice] = int.MaxValue;
            anterior[vertice] = default(T);
        }

        distancia[origem] = 0;
        filaPrioridade.Enqueue(origem);

        while (filaPrioridade.Count > 0)
        {
            T atual = filaPrioridade.Dequeue();

            foreach (var vizinho in grafo.ObterVizinhos(atual))
            {
                T verticeVizinho = vizinho.Key;
                int pesoAresta = vizinho.Value;
                int distanciaNova = distancia[atual] + pesoAresta;

                if (distanciaNova < distancia[verticeVizinho])
                {
                    distancia[verticeVizinho] = distanciaNova;
                    anterior[verticeVizinho] = atual;

                    List<List<T>> paths = todosOsCaminhos.GetValueOrDefault(verticeVizinho, new List<List<T>>());
                    List<T> newPath = new List<T>(todosOsCaminhos.GetValueOrDefault(atual, new List<List<T>> { new List<T>() })[0]);
                    newPath.Add(atual);
                    paths.Add(newPath);
                    todosOsCaminhos[verticeVizinho] = paths;

                    filaPrioridade.Enqueue(verticeVizinho);
                }
            }
        }
    }

    public List<T> GetCaminho(T destino)
    {
        List<T> caminho = new List<T>();
        T atual = destino;

        while (atual != null)
        {
            caminho.Add(atual);
            atual = anterior[atual];
        }

        caminho.Reverse();
        return caminho.Count > 1 ? caminho : null;
    }

    public int GetDistancia(T destino)
    {
        return distancia[destino];
    }

    public List<List<T>> GetTodosOsCaminhos(T destino)
    {
        List<List<T>> paths = todosOsCaminhos.GetValueOrDefault(destino, new List<List<T>>());

        if (paths != null)
        {
            foreach (var path in paths)
            {
                path.Add(destino);
            }
        }

        return paths;
    }
}

public class PriorityQueue<T>
{
    private List<T> list;
    private Comparison<T> comparison;

    public PriorityQueue(Comparison<T> comparison)
    {
        this.list = new List<T>();
        this.comparison = comparison;
    }

    public void Enqueue(T item)
    {
        list.Add(item);
        list.Sort(comparison);
    }

    public T Dequeue()
    {
        T item = list[0];
        list.RemoveAt(0);
        return item;
    }

    public int Count
    {
        get { return list.Count; }
    }
}
