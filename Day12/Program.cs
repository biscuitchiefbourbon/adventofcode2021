using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    class Program
    {
        static void Main(string[] args) {



            String[] textInput = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt");
            Dictionary<String, Cave> cavesByName = new Dictionary<string, Cave>();
            foreach (String mapping in textInput) {
                String[] parts = mapping.Split(new char[] { '-' });
                if (!cavesByName.ContainsKey(parts[0]))
                    cavesByName.Add(parts[0], new Cave(parts[0]));
                if (!cavesByName.ContainsKey(parts[1]))
                    cavesByName.Add(parts[1], new Cave(parts[1]));
                if (!cavesByName[parts[0]].OnwardConnections.Contains(cavesByName[parts[1]]))
                    cavesByName[parts[0]].OnwardConnections.Add(cavesByName[parts[1]]);
                if (!cavesByName[parts[1]].OnwardConnections.Contains(cavesByName[parts[0]]))
                    cavesByName[parts[1]].OnwardConnections.Add(cavesByName[parts[0]]);
            }

            int routeID = 0;
            Cave origin = cavesByName["start"];
            bool partOne = false;
            bool partTwo = true;

            Stack<Node> nodes = new Stack<Node>();
            List<Route> routes = new List<Route>();
            foreach (Cave nextCave in origin.OnwardConnections) {
                //List<Route> startingRoutes = new List<Route>();
                Route newRoute = new Route(routeID++);
                newRoute.Connections.Add(nextCave.Name);
                //startingRoutes.Add(newRoute);
                nodes.Push(new Node(newRoute, nextCave));
            }

            while (nodes.Count > 0) {
                Node node = nodes.Pop();
                foreach (Cave nextCave in node.CurrentCave.OnwardConnections) {
                    if (nextCave.IsStart) {
                        // Ignore, we don't want to go back to the start
                    } else if (nextCave.IsEnd) {
                        // Add route to this point to the final routes
                        Route final = Route.FromExisting(node.RouteToNode, routeID++);
                        final.Connections.Add(nextCave.Name);
                        routes.Add(final);
                    } else {
                        bool doRoute = true;
                        if (partOne && !nextCave.IsLarge && node.RouteToNode.Connections.Contains(nextCave.Name))
                            doRoute = false;
                        else if(partTwo) {
                            if (!nextCave.IsLarge) {
                                if (node.RouteToNode.SmallCaveVisitedTwice()) {
                                    // We have been to a small cave twice already, have we been to this one?
                                    if (node.RouteToNode.Connections.Contains(nextCave.Name)) {
                                        // We have, so we can't go here again
                                        doRoute = false;
                                    }
                                }
                            }
                        }

                        //if (partOne && !nextCave.IsLarge && node.RouteToNode.Connections.Contains(nextCave.Name)) {
                        //    // We have been here and it is a small cave, don't go back
                        //}else if( partTwo && !nextCave.IsLarge && node.RouteToNode.SmallCaveVisitedTwice() && node.RouteToNode.Connections.Contains(nextCave.Name)) {
                        //    // We have already been to a small cave twice
                        //} else {
                        if (doRoute) { 
                            // We are allowed to go here
                            Route newRoute = Route.FromExisting(node.RouteToNode, routeID++);
                            newRoute.Connections.Add(nextCave.Name);
                            nodes.Push(new Node( newRoute , nextCave));
                        }
                        //}

                    }

                }

            }


            Console.WriteLine($"Answer = {routes.Count}");
            Console.ReadLine();


        }

        private class Node
        {
            public Route RouteToNode { get; set; }
            public Cave CurrentCave { get; set; }

            public Node(Route routeToNode, Cave current) {
                RouteToNode = routeToNode;
                CurrentCave = current;
            }
        }


        private class Route
        {
            public int ID { get; set; }
            public List<String> Connections { get; set; }

            public String Current { get { return Connections.Last(); } }

            public bool SmallCaveVisited { get {
                return Connections.Where(c => c != "start" && c != "end" && Char.IsLower(c[0])).Count() > 0;
            } }

            public int GetCaveCount( String name) {
                var counts = Connections.Where(c => c != "start" && Char.IsLower(c[0])).GroupBy(x => x).Select(x => new { Value = x.Key, Count = x.Count() }).ToDictionary(x => x.Value, x => x.Count);
                return counts[name];
            }

            public bool SmallCaveVisitedTwice() {
                return SmallCaveVisited && Connections.Where(c => c != "start" && Char.IsLower(c[0])).GroupBy(x => x).Select(x => new { Value = x.Key, Count = x.Count() }).ToDictionary(x => x.Value, x => x.Count).Values.Max() > 1;
            }

            public Route(int id) {
                ID = id;
                Connections = new List<string>() { "start" };
            }

            public static Route FromExisting(Route existing, int newID) {
                return new Route(newID) { Connections = new List<string>(existing.Connections) };
            }



            public override string ToString() {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Connections.Count; i++) {
                    sb.Append(Connections[i]);
                    if (i < Connections.Count - 1) sb.Append(",");
                }
                return sb.ToString();
            }

        }

        private class Cave
        {
            public string Name { get; set; }

            public bool IsStart { get { return Name == "start"; } }

            public bool IsEnd { get { return Name == "end"; } }

            public bool IsLarge { get { return Char.IsUpper(Name[0]); } }

            public List<int> VisitedRouteNums;

            public List<Cave> OnwardConnections { get; set; }

            public Cave(string name) {
                Name = name;
                OnwardConnections = new List<Cave>();
                VisitedRouteNums = new List<int>();
            }

            public override bool Equals(object obj) {
                Cave other = obj as Cave;
                return Name == other.Name;
            }

            public override string ToString() {
                return Name;
            }
        }
    }
}
