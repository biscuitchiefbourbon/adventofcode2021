using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Program
    {
        static void Main(string[] args) {
            //int xVelocity = 17, yVelocity = -4;            
            int minX = 185, maxX = 221, minY = -122, maxY = -74;
            //int minX = 20, maxX = 30, minY = -10, maxY = -5;

            int highestY = int.MinValue;
            int bestStartX = 0, bestStartY = 0;
            int totalHits = 0;

            for (int xVelocity = -1000; xVelocity < 1000; xVelocity++) {
                for( int yVelocity = -1000; yVelocity < 1000; yVelocity++) {
                    

                    int workingxVelocity = xVelocity, workingyVelocity = yVelocity;    
                    int x = 0, y = 0, workingHighY = 0 ;
                    bool done = false;
                    int step = 1;
                    while (!done) {
                        x += workingxVelocity;
                        y += workingyVelocity;
                        if (workingxVelocity > 0) {
                            workingxVelocity--;
                        } else if (workingxVelocity < 0) {
                            workingxVelocity++;
                        }
                        workingyVelocity--;
                        if (y > workingHighY)
                            workingHighY = y;

                        //Console.WriteLine($"After step {step++} x = {x}, y={y}, workingxVelocity={workingxVelocity}, workingyVelocity={workingyVelocity}");
                        if (x >= minX && x <= maxX && y >= minY && y <= maxY) {
                            //Console.WriteLine("Target area hit");
                            if (workingHighY > highestY) {
                                highestY = workingHighY;
                                bestStartX = xVelocity;
                                bestStartY = yVelocity;
                                Console.WriteLine($"New highest y = {highestY} from {bestStartX},{bestStartY}");
                            }
                            Console.WriteLine($"Target hit, y = {workingHighY} from {xVelocity},{yVelocity}");
                            totalHits++;
                            done = true;
                        } else if (x > maxX || y < minY) {
                            done = true;
                            //Console.WriteLine("Target area missed");
                        }
                        step++;
                    }
                }
            }

            Console.WriteLine($"Done, total hits = {totalHits}");
            Console.ReadLine();

        }
    }
}
