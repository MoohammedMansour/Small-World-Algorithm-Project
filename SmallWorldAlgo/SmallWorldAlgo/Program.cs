//بسم الله الرحمن الرحيم 
// ربنا يعدي البروجيكت دا ع خير 
using System;
using System.Collections.Generic; //to use hashset
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 
using System.Data;
using System.Diagnostics;

namespace SmallWorldAlgo
{
    class Program
    {
        public class Queries
        {

            public string Actor_Number1;
            public string Actor_Number2;

        }
        public class Movie_Details
        {

            public string movie_name;

            public List<string> list_actors;
            //constructor to intial values
            public Movie_Details()  
            {

                list_actors = new List<string>();

            }

        }
        // Class Carry All Data About Node which is Passed IN  Breadth fast search (bfs)
        public class node
        {
            public int index;
            public int strength;
            public int level;
            public int index_check;

            public node(int index, int strength, int level, int checker)
            {
                this.index = index;
                this.strength = strength;
                this.level = level;
                this.index_check = checker;
            }
            public node()
            {

            }

        }
        // To Count all Actors in search
        static int count_actors = 0;
        //All Movies Return in List 
        public static List<Movie_Details> movies = new List<Movie_Details>();
        //Save all actors in hashtable and remove all redundant 
        public static HashSet<string> actors = new HashSet<string>();
        //relate actors to names  
        public static Dictionary<string, int> actors_active = new Dictionary<string, int>();
        //relate actors to names reversed 
        public static Dictionary<int, string> actors_active_reversed = new Dictionary<int, string>();
        // adjacency list ; a vector of maps
        public static List<Dictionary<int, int>> adjacent_list = new List<Dictionary<int, int>>();
        //Movies Which Shared With 2 Actors 
        public static List<Dictionary<string, string>> shared_movie = new List<Dictionary<string, string>>();
        //Actors Which Shared With 2 Actors 
        public static List<Dictionary<string, string>> shared_actors = new List<Dictionary<string, string>>();

        public static void graph_representation() // Total Complexity is 
        {   
            for (int i = 0; i < actors.Count(); i++) //O(N)+O(1)
            { 
                adjacent_list.Add(new Dictionary<int, int>());  //O(1)
                shared_movie.Add(new Dictionary<string, string>()); //O(1)
                shared_actors.Add(new Dictionary<string, string>()); //O(1)
            }
            //Give each actor a number and Reversing it .
            foreach (string item in actors)  //O(N) 
            {

                actors_active.Add(item, count_actors); //O(1) 
                actors_active_reversed.Add(count_actors, item); //O(1) 
                count_actors++; //O(1)

            } 
            //Looping in Movies   
            foreach (Movie_Details item in movies) //O(N) * O(N) * O(N) = O(N)^3
            {
                //accessing each actor in that movie once as a primary index
                for (int i = 0; i < item.list_actors.Count(); i++) //O(N) + 
                {   
                    // increase the graph weight To Calculate.
                    for (int n = 0; n < item.list_actors.Count(); n++)  //O(N) + O(1) 
                    {


                        if (item.list_actors[i] != item.list_actors[n] /* check if both comparirators are not on the same index*/) //O(1) 
                        {
                            int temp1 = actors_active[item.list_actors[i]]; //O(1)
                            int temp2 = actors_active[item.list_actors[n]]; //O(1)
                            if (adjacent_list[temp1].ContainsKey(temp2) == true) //O(1)
                                //increase weight here 
                                adjacent_list[temp1][temp2]++; //O(1)


                            else //O(1)
                            {

                                shared_movie[temp1].Add(actors_active_reversed[temp2], item.movie_name);//O(1)
                                adjacent_list[temp1].Add(temp2, 1);//O(1)
                                shared_actors[temp1].Add(actors_active_reversed[temp2] , item.movie_name);//O(1)

                            }

                        }

                    }

                }


            }
        }
        static void breadth_fast_search (string actor1, string actor2) //Total Complexity is 
        {
            //This Queue for Save return Values    
            Queue<int> queue = new Queue<int>();//O(1)
            Dictionary<int, node> all_nodes = new Dictionary<int, node>();//O(1)
            // Point1 node index
            int point1_actor = actors_active[actor1];//O(1)
            // Point2 node index
            int point2_actor = actors_active[actor2];//O(1)
            bool destination_point = false;//O(1)
            queue.Enqueue(point1_actor);//O(1)
            node source_node = new node(point1_actor, 0, 0, -1);//O(1)
            all_nodes.Add(point1_actor, source_node);//O(1)
            // node of next 
            node away_node = new node(); //O(1)
            while (queue.Any() /*To check IF the Queue is Empty Or Not */) 
            {   
                int parent_index_node = queue.Dequeue(); //O(1)
                // Here Check Minimum Path Level 
                if (destination_point && (all_nodes[parent_index_node].level >= away_node.level)) 

                {

                    Console.Write(actors_active_reversed[point1_actor] + "/" + actors_active_reversed[point2_actor] + "\t" + away_node.level + "\t" + away_node.strength + "\t");
                   //Create a stack to reverse a points 
                    Stack<int> output = new Stack<int>();//O(1)

                    // the stack is to unreverse it
                    node current_node = new node();
                    output.Push(away_node.index);
                    current_node = away_node;

                    for (int n = 0; n < away_node.level; n++)
                    {
                        current_node = all_nodes[current_node.index_check];
                        output.Push(current_node.index);

                    }

                    int actor1Index = output.Pop();
                    int actor2Index = output.Peek();


                    //Console.WriteLine();
                    Console.Write(shared_movie[actor1Index][actors_active_reversed[actor2Index]]);
                    while (output.Count() > 1)
                    {
                        Console.Write(" => ");

                        actor1Index = output.Pop();
                        //peak return the head of stack output 
                        actor2Index = output.Peek();
                        Console.Write(shared_movie[actor1Index][actors_active_reversed[actor2Index]]);


                        //Console.Write(shared_actors[actor1Index][actors_active_reversed[actor2Index]]);


                    }
                    //Console.WriteLine();
                    Console.WriteLine();
                    Console.Write(actor1 + "=>" + actor2);
                    Console.WriteLine();
                    break;
                }
                //To Find The strength
                foreach (KeyValuePair<int, int> item in adjacent_list[parent_index_node])
                {
                    node current_node = new node();

                    if (!all_nodes.ContainsKey(item.Key))
                    {
                        current_node = new node(item.Key, all_nodes[parent_index_node].strength + item.Value, all_nodes[parent_index_node].level + 1, parent_index_node);
                        all_nodes.Add(item.Key, current_node);
                        queue.Enqueue(item.Key);
                    }
                    // Make The Point2 IS carry an object of point 1 
                    if (item.Key == point2_actor && !destination_point)
                    {
                        away_node = current_node;
                        destination_point = true;
                    }
                  

                    if (item.Key == point2_actor && destination_point && ((all_nodes[parent_index_node].level + 1) <= away_node.level))
                    {
                        if (all_nodes[parent_index_node].strength + item.Value > away_node.strength)
                            away_node.strength = all_nodes[parent_index_node].strength + item.Value;
                    }

                }
            }
        }
        public static void load_input_file(string path1) //Total Complexity is 
        {
            //Access the movies data file
            FileStream movies_data_file = new FileStream(path1, FileMode.Open, FileAccess.Read); //O(1)
            //Read the File
            StreamReader movies_Reading = new StreamReader(movies_data_file);//O(1)
            //To read line by line
            string single_line = null;//O(1)
            while (movies_Reading.Peek() != -1) //O(N*k)
            {
                Movie_Details movie = new Movie_Details();//O(1)
                single_line = movies_Reading.ReadLine(); //O(1)
                string[] line_splited = single_line.Split('/'); //O(k) which k is the number of input 
                movie.movie_name = line_splited[0];//O(1)
                for (/*0 index is the movie name */int i = 1; i < line_splited.Count(); i++) //O(N)
                {
                    actors.Add(line_splited[i]); //O(1)
                    movie.list_actors.Add(line_splited[i]); //O(1)
                }
                movies.Add(movie); //O(1)
            }
            movies_Reading.Close(); //O(1)
            graph_representation(); //O(N^3)
        }
        public static void load_queries(string path2) // Total Complexity is 
        {
            string actors = null; 
            FileStream queries_data_file = new FileStream(path2, FileMode.Open, FileAccess.Read);

            StreamReader queries_Reading = new StreamReader(queries_data_file);
            while (queries_Reading.Peek() != -1) //O(Bfs + k )
            {
                Queries my_querie = new Queries();//o(1)
                actors = queries_Reading.ReadLine();//o(1)
                string[] actors_splited = actors.Split('/'); //O(k) whick is the number of input file  
                my_querie.Actor_Number1 = actors_splited[0]; //o(1)
                my_querie.Actor_Number2= actors_splited[1];//o(1)
                Console.WriteLine("qeuery" + "\t" + "DoS." + "\t" + "Rs." + "\t" + "Chain of Movies." + "\t" + "Chain of Actors"  );
                //Complexity of BFS 
                breadth_fast_search(my_querie.Actor_Number1, my_querie.Actor_Number2);
            }
            queries_Reading.Close(); //o(1)
        }    
        static void Main(string[] args)
        {
            Console.WriteLine("==================== WELCOME IN TEST CASES  ==========================");
            Console.WriteLine(" =================== 1 : To simple Test ================= ");
            Console.WriteLine(" =================== 2 : To complete Test =============== ");
            string x = Console.ReadLine();
            Console.WriteLine(x);
            if (x=="1")
            {
                load_input_file("Testcases/Sample/movies1.txt");
                load_queries("Testcases/Sample/queries1.txt");
                Console.WriteLine(" Test is Finish ");
                Console.WriteLine(" Enter 0 To Close Program ");
                string y = Console.ReadLine();
            }
            else if (x == "2")
            {
                Console.WriteLine("1 = For Small Test Case");
                Console.WriteLine("2 = For Meduim Test Case");
                Console.WriteLine("3 = For Large Test Case");
                Console.WriteLine("4 = For Extreme Test Case");
                Console.WriteLine(" ========================================== ");
                Console.WriteLine(" ========================================== ");
                string c = Console.ReadLine();
                if (c == "1")
                {
                    Console.WriteLine(" 1- Case '1'");
                    Console.WriteLine(" 2- Case '2'");
                    string n = Console.ReadLine();
                    if (n =="1")
                    {
                        load_input_file("Testcases/Complete/small/Case1/Movies193.txt");
                        load_queries("Testcases/Complete/small/Case1/queries110.txt");
                        Console.WriteLine(" Test is Finish ");
                        Console.WriteLine(" Enter 0 To Close Program ");
                        string y = Console.ReadLine();
                    }
                    else
                    {
                        load_input_file("Testcases/Complete/small/Case2/Movies187.txt");
                        load_queries("Testcases/Complete/small/Case2/queries50.txt");
                        Console.WriteLine(" Test is Finish ");
                        Console.WriteLine(" Enter 0 To Close Program ");
                        string y = Console.ReadLine();
                    }


                   
                }
                else if(c =="2"){
                    Console.WriteLine(" 1- Case '1'");
                    Console.WriteLine(" 2- Case '2'");
                    string n = Console.ReadLine();
                    if (n == "1")
                    {
                        load_input_file("Testcases/Complete/medium/Case1/Movies967.txt");
                        load_queries("Testcases/Complete/medium/Case1/queries4000.txt");
                        Console.WriteLine(" Test is Finish ");
                        Console.WriteLine(" Enter 0 To Close Program ");
                        string y = Console.ReadLine();
                    }
                    else if (n=="2")
                    {
                        load_input_file("Testcases/Complete/medium/Case2/Movies4736.txt");
                        load_queries("Testcases/Complete/medium/Case2/queries2000.txt");
                        Console.WriteLine(" Test is Finish ");
                        Console.WriteLine(" Enter 0 To Close Program ");
                        string y = Console.ReadLine();
                    }
               
                   
                }
                else if (c == "3")
                {
                    Console.WriteLine(" 1- Case '1'");
                    Console.WriteLine(" 2- Case '2'");
                    string n = Console.ReadLine();
                    if (n == "1")
                    {
                        load_input_file("Testcases/Complete/large/Movies14129.txt");
                        load_queries("Testcases/Complete/large/queries26.txt");
                        Console.WriteLine(" Test is Finish ");
                        Console.WriteLine(" Enter 0 To Close Program ");
                        string y = Console.ReadLine();
                    }
                    else if (n == "2")
                    {
                        load_input_file("Testcases/Complete/large/Movies14129.txt");
                        load_queries("Testcases/Complete/large/queries600.txt");
                        Console.WriteLine(" Test is Finish ");
                        Console.WriteLine(" Enter 0 To Close Program ");
                        string y = Console.ReadLine();
                    }
                }
                else if (c=="4")
                {
                    Console.WriteLine(" 1- Case '1'");
                    Console.WriteLine(" 2- Case '2'");
                    string n = Console.ReadLine();
                    if (n == "1")
                    {
                        load_input_file("Testcases/Complete/extreme/Movies122806.txt");
                        load_queries("Testcases/Complete/extreme/queries22.txt");
                        Console.WriteLine(" Test Case Finish ");
                        Console.WriteLine(" Test Case has 3.9 Minutes ");
                        Console.WriteLine(" Enter 0 To Close Program ");
                        string y = Console.ReadLine();
                    }
                    else if (n == "2")
                    {
                        load_input_file("Testcases/Complete/extreme/Movies122806.txt");
                        load_queries("Testcases/Complete/extreme/queries200.txt");
                        Console.WriteLine(" Test is Finish ");
                        Console.WriteLine(" Enter 0 To Close Program ");
                        string y = Console.ReadLine();
                    }
                }
                


            }
            else if (x == "3")
            {

                TextWriterTraceListener tr2 = new TextWriterTraceListener(System.IO.File.CreateText("Output1.txt"));
                Debug.Listeners.Add(tr2);
            }






        
    }
    }
}