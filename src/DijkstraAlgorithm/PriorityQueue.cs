using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    //Making our own Priority Queue using a binary tree to prioritize T values
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private T[] tree = new T[30];
        private IComparer<T> comparer;
        private int count;
        public int Count { get { return count + 1; } }
        public T this[int i]
        {
            get
            {
                if (i > -1 && i < count)
                {
                    return tree[i];
                }

                throw new ArgumentOutOfRangeException();
            }
        }

        public PriorityQueue(IComparer<T> comparer)
        {
            this.comparer = comparer ?? Comparer<T>.Default;
            count = 0;
        }
      
        //Adding to the queue
        public void Enqueue(T value)
        {
            //Increase the count then check if we hit the max, if so increase the tree
            count++;

            if (count == tree.Length)
            {
                IncreaseTree();
            }

            //Add the value to the last available index then Heapify Up the tree to add it to the right place
            tree[count] = value;

            HeapifyUp(count);
        }

        //Removing from the queue
        public T Dequeue()
        {
            //Grab the priority value
            T root = tree[1];

            //Root equals to the last value then the last value is now empty
            tree[1] = tree[count];
            tree[count] = default(T);

            count--;

            //Heapify the tree down so everything is in order
            HeapifyDown(1);

            return root;
        }        

        //Check if there are any values in the queue
        public bool IsEmpty()
        {
            return count == 0;
        }

        //If the queueu contains a certain value
        public bool Contains(T item)
        {
            return tree.Contains(item);
        }

        //Recursive function to heapify up from the tree to prioritize the queue
        private void HeapifyUp(int index)
        {
            //Get the parent index
            int parent = index / 2;

            //If the parent is the root or the parent is the same as the left child of the root or the parent is the same as the index exit the function
            if (parent < 1 || comparer.Compare(tree[parent], tree[1]) == 0 || comparer.Compare(tree[index], tree[parent]) == 0)
            {
                return;
            }

            //If the index is smaller than the parent move the index up
            if (comparer.Compare(tree[index], tree[parent]) < 0)
            {
                T temp = tree[index];
                tree[index] = tree[parent];
                tree[parent] = temp;
            }

            //Keep going up the tree until we have reached the root
            HeapifyUp(parent);
        }

        private void HeapifyDown(int index)
        {
            //Grab the children of the node
            int leftChild = index * 2;
            int rightChild = index * 2 + 1;

            int swapIndex = 0;

            //If we hit the end of the tree exit
            if (leftChild > count || rightChild > count)
            {
                return;
            }

            //If the left child is smaller than the right child it swaps and visa versa
            if (comparer.Compare(tree[leftChild], tree[rightChild]) < 0)
            {
                swapIndex = leftChild;
            }
            else
            {
                swapIndex = rightChild;
            }

            //If the swap child is smaller than the parent swap them
            if (comparer.Compare(tree[swapIndex], tree[index]) < 0)
            {
                T temp = tree[index];
                tree[index] = tree[swapIndex];
                tree[swapIndex] = temp;
            }

            //Keep doing down the tree
            HeapifyDown(swapIndex);
        }

        //Double the size of the tree when increasing it
        private void IncreaseTree()
        {
            T[] temp = new T[tree.Length * 2];
            tree.CopyTo(temp, 0);
            tree = temp;
        }

        //Display all the values in the queue
        public override string ToString()
        {
            string text = "";

            for (int i = 1; i < Count; i++)
            {
                text += tree[i] + " ";
            }

            return text;
        }
    }
}
