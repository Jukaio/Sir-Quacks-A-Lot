using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// My first binary tree and AVL implementation

namespace David
{
    // Binary_Tree
    public class Node<T> where T : IComparable
    {
        public Node(T p_data)
        {
            m_data = p_data;
        }

        private T m_data;
        public virtual T Data { get { return m_data; } set { m_data = value; } }

        private Node<T> m_left;
        public virtual Node<T> Left { get { return m_left; } set { m_left = value; } }

        private Node<T> m_right;
        public virtual Node<T> Right { get { return m_right; } set { m_right = value; } }

        private Node<T> m_parent;
        public virtual Node<T> Parent { get { return m_parent; } set { m_parent = value; } }

        Binary_Tree<T> m_tree;
        public virtual Binary_Tree<T> Tree { get { return m_tree; } set { m_tree = value; } }

        public virtual bool Is_Leaf { get { return Child_Count == 0; } }
        public virtual bool Has_Children { get { return Child_Count > 0; } }
        public virtual bool Is_Left_Child { get { return Parent != null && Parent.Left == this; } }
        public virtual bool Is_Right_Child { get { return Parent != null && Parent.Right == this; } }

        public virtual bool Has_Left_Child { get { return Left != null; } }
        public virtual bool Has_Right_Child { get { return Right != null; } }

        public virtual int Child_Count
        {
            get
            {
                int count = 0;

                if (Left != null)
                    count++;

                if (Right != null)
                    count++;

                return count;
            }
        }
    }
    public class Binary_Tree<T> : ICollection<T> where T : IComparable
    {
        public Binary_Tree()
        {
            m_root = null;
            m_size = 0;
        }

        public enum Traversal_Mode
        {
            IN_ORDER = 0,
            POST_ORDER,
            PRE_ORDER,
        }
        private Traversal_Mode m_traversal_mode = Traversal_Mode.IN_ORDER;
        public virtual Traversal_Mode Traversal_Order { get { return m_traversal_mode; } set { m_traversal_mode = value; } }

        private Node<T> m_root;
        public virtual Node<T> Root { get { return m_root; } set { m_root = value; } }

        public virtual bool IsReadOnly { get { return false; } }

        private int m_size;
        public virtual int Count { get { return m_size; } }

        public virtual int Height
        {
            get { return this.Get_Height(this.m_root); }
        }
        public virtual int Get_Height(T p_data)
        {
            Node<T> node = Find(p_data);
            if (node != null)
                return Get_Height(node);
            else
                return 0;
        }
        public virtual int Get_Height(Node<T> p_start_node)
        {
            if (p_start_node == null)
                return 0;
            else
                return 1 + Math.Max(Get_Height(p_start_node.Left), Get_Height(p_start_node.Right));
        }

        public virtual int Get_Depth(T p_data)
        {
            Node<T> node = Find(p_data);
            return Get_Depth(node);
        }

        public virtual int Get_Depth(Node<T> p_node)
        {
            int depth = 0;

            if (p_node == null)
                return depth;

            Node<T> parent = p_node.Parent;
            while (parent != null)
            {
                depth++;
                parent = parent.Parent;
            }

            return depth;
        }

        private Comparison<IComparable> m_comparer = CompareElements;

        public virtual void Add(T p_item)
        {
            Node<T> node = new Node<T>(p_item);
            Add(node);
        }

        public virtual void Add(Node<T> p_node)
        {
            if (m_root == null)
            {
                m_root = p_node;
                m_root.Tree = this;
                m_size++;
            }
            else
            {
                if (p_node.Parent == null)
                    p_node.Parent = m_root;

                bool insert_on_left = m_comparer((IComparable)p_node.Data, (IComparable)p_node.Parent.Data) <= 0;

                if (insert_on_left)
                {
                    if (p_node.Parent.Left == null)
                    {
                        p_node.Parent.Left = p_node;
                        p_node.Tree = this;
                        m_size++;
                    }
                    else
                    {
                        p_node.Parent = p_node.Parent.Left;
                        Add(p_node);
                    }
                }
                else
                {
                    if (p_node.Parent.Right == null)
                    {
                        p_node.Parent.Right = p_node;
                        p_node.Tree = this;
                        m_size++;
                    }
                    else
                    {
                        p_node.Parent = p_node.Parent.Right;
                        Add(p_node);
                    }
                }
            }
        }

        public virtual Node<T> Find(T p_data)
        {
            Node<T> node = m_root;
            while (node != null)
            {
                if (node.Data.Equals(p_data))
                    return node;

                else
                {
                    bool search_left = m_comparer((IComparable)p_data, (IComparable)node.Data) < 0;

                    if (search_left)
                        node = node.Left;
                    else
                        node = node.Right;
                }
            }
            return null;
        }

        public virtual bool Contains(T p_data)
        {
            return (Find(p_data) != null);
        }

        public virtual bool Remove(T p_data)
        {
            Node<T> remove_node = Find(p_data);
            return Remove(remove_node);
        }

        public virtual bool Remove(Node<T> p_node)
        {
            if (p_node == null || p_node.Tree != this)
                return false;

            bool was_root = (p_node == m_root);

            if (Count == 1) // Node is root
            {
                m_root = null;
                p_node.Tree = null;

                m_size--;
            }
            else if (p_node.Is_Leaf) // Node has no children
            {
                if (p_node.Is_Left_Child)
                    p_node.Parent.Left = null;
                else
                    p_node.Parent.Right = null;

                p_node.Tree = null;
                p_node.Parent = null;

                m_size--;
            }
            else if (p_node.Child_Count == 1) // Node has one child
            {
                if (p_node.Has_Left_Child)
                {
                    p_node.Left.Parent = p_node.Parent;

                    if (was_root)
                        m_root = p_node.Left;

                    if (p_node.Is_Left_Child)
                        p_node.Parent.Left = p_node.Left;
                    else
                        p_node.Parent.Right = p_node.Left;
                }
                else
                {
                    p_node.Right.Parent = p_node.Parent;

                    if (was_root)
                        m_root = p_node.Right;

                    if (p_node.Is_Left_Child)
                        p_node.Parent.Left = p_node.Right;
                    else
                        p_node.Parent.Right = p_node.Right;
                }

                p_node.Tree = null;
                p_node.Parent = null;
                p_node.Left = null;
                p_node.Right = null;
            }
            else // Two children
            {
                Node<T> successor = p_node.Left;

                while (successor.Right != null)
                {
                    successor = successor.Right;
                }

                p_node.Data = successor.Data;
                Remove(successor);
            }

            return true;
        }

        // Finish binary tree -> Make AVL Binary tree (Balanced tree) 
        public void Clear()
        {

            IEnumerator<T> enumerator = Get_Post_Order_Enumerator();
            while (enumerator.MoveNext())
            {
                Remove(enumerator.Current);
            }
            enumerator.Dispose();
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            switch (m_traversal_mode)
            {
                case Traversal_Mode.IN_ORDER:
                    return Get_In_Order_Enumertor();
                case Traversal_Mode.POST_ORDER:
                    return Get_Post_Order_Enumerator();
                case Traversal_Mode.PRE_ORDER:
                    return Get_Pre_Order_Enumerator();
                default:
                    return Get_In_Order_Enumertor();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual IEnumerator<T> Get_In_Order_Enumertor()
        {
            return new Binary_Tree_In_Order_Enumerator(this);
        }

        public virtual IEnumerator<T> Get_Post_Order_Enumerator()
        {
            return new Binary_Tree_Post_Order_Enumerator(this);
        }

        public virtual IEnumerator<T> Get_Pre_Order_Enumerator()
        {
            return new Binary_Tree_Pre_Order_Enumerator(this);
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        public virtual void CopyTo(T[] p_array, int p_start)
        {
            IEnumerator<T> enumerator = GetEnumerator();

            for (int i = p_start; i < p_array.Length; i++)
            {
                if (enumerator.MoveNext())
                    p_array[i] = enumerator.Current;
                else
                    break;
            }
        }

        public static int CompareElements(IComparable x, IComparable y)
        {
            return x.CompareTo(y);
        }

        internal class Binary_Tree_In_Order_Enumerator : IEnumerator<T>
        {
            private Node<T> m_current;
            private Binary_Tree<T> m_tree;
            internal Queue<Node<T>> m_traverse_queue;

            public Binary_Tree_In_Order_Enumerator(Binary_Tree<T> tree)
            {
                this.m_tree = tree;

                //Build queue
                m_traverse_queue = new Queue<Node<T>>();
                visitNode(m_tree.Root);
            }

            private void visitNode(Node<T> node)
            {
                if (node == null)
                    return;
                else
                {
                    visitNode(node.Left);
                    m_traverse_queue.Enqueue(node);
                    visitNode(node.Right);
                }
            }

            public T Current
            {
                get { return m_current.Data; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                m_current = null;
                m_tree = null;
            }

            public void Reset()
            {
                m_current = null;
            }

            public bool MoveNext()
            {
                if (m_traverse_queue.Count > 0)
                    m_current = m_traverse_queue.Dequeue();
                else
                    m_current = null;

                return (m_current != null);
            }


        }

        internal class Binary_Tree_Post_Order_Enumerator : IEnumerator<T>
        {
            private Node<T> m_current;
            private Binary_Tree<T> m_tree;
            internal Queue<Node<T>> m_traverse_queue;

            public Binary_Tree_Post_Order_Enumerator(Binary_Tree<T> tree)
            {
                this.m_tree = tree;

                //Build queue
                m_traverse_queue = new Queue<Node<T>>();
                visitNode(this.m_tree.Root);
            }

            private void visitNode(Node<T> node)
            {
                if (node == null)
                    return;
                else
                {
                    visitNode(node.Left);
                    visitNode(node.Right);
                    m_traverse_queue.Enqueue(node);
                }
            }

            public T Current
            {
                get { return m_current.Data; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                m_current = null;
                m_tree = null;
            }

            public void Reset()
            {
                m_current = null;
            }

            public bool MoveNext()
            {
                if (m_traverse_queue.Count > 0)
                    m_current = m_traverse_queue.Dequeue();
                else
                    m_current = null;

                return (m_current != null);
            }
        }

        internal class Binary_Tree_Pre_Order_Enumerator : IEnumerator<T>
        {
            private Node<T> m_current;
            private Binary_Tree<T> m_tree;
            internal Queue<Node<T>> m_traverse_queue;

            public Binary_Tree_Pre_Order_Enumerator(Binary_Tree<T> tree)
            {
                this.m_tree = tree;

                //Build queue
                m_traverse_queue = new Queue<Node<T>>();
                visitNode(this.m_tree.Root);
            }

            private void visitNode(Node<T> node)
            {
                if (node == null)
                    return;
                else
                {
                    m_traverse_queue.Enqueue(node);
                    visitNode(node.Left);
                    visitNode(node.Right);
                }
            }

            public T Current
            {
                get { return m_current.Data; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                m_current = null;
                m_tree = null;
            }

            public void Reset()
            {
                m_current = null;
            }

            public bool MoveNext()
            {
                if (m_traverse_queue.Count > 0)
                    m_current = m_traverse_queue.Dequeue();
                else
                    m_current = null;

                return (m_current != null);
            }
        }
    }

    // AVL_Tree
    public class AVL_Node<T> : Node<T> where T : IComparable
    {
        public AVL_Node(T p_value)
            : base(p_value)
        {

        }

        public new AVL_Node<T> Left
        {
            get
            {
                return (AVL_Node<T>)base.Left;
            }
            set
            {
                base.Left = value;
            }
        }

        public new AVL_Node<T> Right
        {
            get
            {
                return (AVL_Node<T>)base.Right;
            }
            set
            {
                base.Right = value;
            }
        }

        public new AVL_Node<T> Parent
        {
            get
            {
                return (AVL_Node<T>)base.Parent;
            }
            set
            {
                base.Parent = value;
            }
        }
    }
    public class AVL_Tree<T> : Binary_Tree<T> where T : IComparable
    {
        public new AVL_Node<T> Root
        {
            get { return (AVL_Node<T>)base.Root; }
            set { base.Root = value; }
        }

        public new AVL_Node<T> Find(T p_data)
        {
            return (AVL_Node<T>)base.Find(p_data);
        }

        public override void Add(T p_data)
        {
            AVL_Node<T> node = new AVL_Node<T>(p_data);

            base.Add(node);
            AVL_Node<T> parent = node.Parent;

            while (parent != null)
            {
                int balance = Get_Balance(parent);
                if (Math.Abs(balance) == 2)
                {
                    Balance_At(parent, balance);
                }

                parent = parent.Parent;
            }
        }

        public override bool Remove(T p_data)
        {
            AVL_Node<T> node = Find(p_data);
            return Remove(node);
        }

        protected new bool Remove(Node<T> p_node)
        {
            return Remove((AVL_Node<T>)p_node);
        }

        public bool Remove(AVL_Node<T> p_node)
        {
            AVL_Node<T> parent = p_node.Parent;

            bool removed = base.Remove(p_node);

            if (!removed)
                return false;
            else
            {
                while (parent != null)
                {
                    int balance = Get_Balance(parent);

                    if (Math.Abs(balance) == 1)
                    {
                        break;
                    }
                    else if (Math.Abs(balance) == 2)
                    {
                        Balance_At(parent, balance);
                    }

                    parent = parent.Parent;
                }

                return true;
            }
        }

        protected virtual void Balance_At(AVL_Node<T> p_node, int balance)
        {
            if (balance == 2)
            {
                int right_balance = Get_Balance(p_node.Right);

                if (right_balance == 1 || right_balance == 0)
                {
                    Rotate_Left(p_node);
                }
                else if (right_balance == -1)
                {
                    Rotate_Right(p_node.Right);

                    Rotate_Left(p_node);
                }
            }
            else if (balance == -2)
            {
                int left_balance = Get_Balance(p_node.Left);
                if (left_balance == 1)
                {
                    Rotate_Left(p_node.Left);

                    Rotate_Right(p_node);
                }
                else if (left_balance == -1 || left_balance == 0)
                {
                    Rotate_Right(p_node);
                }
            }
        }

        protected virtual int Get_Balance(AVL_Node<T> p_root)
        {
            return Get_Height(p_root.Right) - Get_Height(p_root.Left);
        }

        protected virtual void Rotate_Left(AVL_Node<T> p_root)
        {
            if (p_root == null)
                return;

            AVL_Node<T> pivot = p_root.Right;

            if (pivot == null)
                return;
            else
            {
                AVL_Node<T> root_parent = p_root.Parent;
                bool is_left_child = (root_parent != null) && root_parent.Left == p_root;
                bool make_tree_root = p_root.Tree.Root == p_root;

                p_root.Right = pivot.Left;
                pivot.Left = p_root;

                p_root.Parent = pivot;
                pivot.Parent = root_parent;

                if (p_root.Right != null)
                    p_root.Right.Parent = p_root;

                if (make_tree_root)
                {
                    pivot.Tree.Root = pivot;
                }

                if (is_left_child)
                {
                    root_parent.Left = pivot;
                }
                else if (root_parent != null)
                {
                    root_parent.Right = pivot;
                }
            }
        }

        protected virtual void Rotate_Right(AVL_Node<T> p_root)
        {
            if (p_root == null)
                return;

            AVL_Node<T> pivot = p_root.Left;

            if (pivot == null)
                return;
            else
            {
                AVL_Node<T> root_parent = p_root.Parent;
                bool is_left_child = (root_parent != null) && root_parent.Left == p_root;
                bool make_tree_root = p_root.Tree.Root == p_root;

                p_root.Left = pivot.Right;
                pivot.Right = p_root;

                p_root.Parent = pivot;
                pivot.Parent = root_parent;

                if (p_root.Left != null)
                    p_root.Left.Parent = p_root;

                if (make_tree_root)
                {
                    pivot.Tree.Root = pivot;
                }

                if (is_left_child)
                {
                    root_parent.Left = pivot;
                }
                else if (root_parent != null)
                {
                    root_parent.Right = pivot;
                }
            }
        }
    }
}

//====================================================
//| Downloaded From                                  |
//| Visual C# Kicks - http://www.vcskicks.com/       |
//| License - http://www.vcskicks.com/license.html   |
//====================================================
 
namespace CSKicksCollection.Trees
{
    /// <summary>
    /// A Binary Tree node that holds an element and references to other tree nodes
    /// </summary>
    class Node<T>
        where T : IComparable
    {
        private T value;
        private Node<T> leftChild;
        private Node<T> rightChild;
        private Node<T> parent;
        private BinaryTree<T> tree;

        /// <summary>
        /// The value stored at the node
        /// </summary>
        public virtual T Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Gets or sets the left child node
        /// </summary>
        public virtual Node<T> LeftChild
        {
            get { return leftChild; }
            set { leftChild = value; }
        }

        /// <summary>
        /// Gets or sets the right child node
        /// </summary>
        public virtual Node<T> RightChild
        {
            get { return rightChild; }
            set { rightChild = value; }
        }

        /// <summary>
        /// Gets or sets the parent node
        /// </summary>
        public virtual Node<T> Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// Gets or sets the Binary Tree the node belongs to
        /// </summary>
        public virtual BinaryTree<T> Tree
        {
            get { return tree; }
            set { tree = value; }
        }

        /// <summary>
        /// Gets whether the node is a leaf (has no children)
        /// </summary>
        public virtual bool IsLeaf
        {
            get { return this.ChildCount == 0; }
        }

        /// <summary>
        /// Gets whether the node is internal (has children nodes)
        /// </summary>
        public virtual bool IsInternal
        {
            get { return this.ChildCount > 0; }
        }

        /// <summary>
        /// Gets whether the node is the left child of its parent
        /// </summary>
        public virtual bool IsLeftChild
        {
            get { return this.Parent != null && this.Parent.LeftChild == this; }
        }

        /// <summary>
        /// Gets whether the node is the right child of its parent
        /// </summary>
        public virtual bool IsRightChild
        {
            get { return this.Parent != null && this.Parent.RightChild == this; }
        }

        /// <summary>
        /// Gets the number of children this node has
        /// </summary>
        public virtual int ChildCount
        {
            get
            {
                int count = 0;

                if (this.LeftChild != null)
                    count++;

                if (this.RightChild != null)
                    count++;

                return count;
            }
        }

        /// <summary>
        /// Gets whether the node has a left child node
        /// </summary>
        public virtual bool HasLeftChild
        {
            get { return (this.LeftChild != null); }
        }

        /// <summary>
        /// Gets whether the node has a right child node
        /// </summary>
        public virtual bool HasRightChild
        {
            get { return (this.RightChild != null); }
        }

        /// <summary>
        /// Create a new instance of a Binary Tree node
        /// </summary>
        public Node(T value)
        {
            this.value = value;
        }
    }

    /// <summary>
    /// Binary Tree data structure
    /// </summary>
    class BinaryTree<T> : ICollection<T>
        where T : IComparable
    {
        /// <summary>
        /// Specifies the mode of scanning through the tree
        /// </summary>
        public enum TraversalMode
        {
            InOrder = 0,
            PostOrder,
            PreOrder
        }

        private Node<T> head;
        private Comparison<IComparable> comparer = CompareElements;
        private int size;
        private TraversalMode traversalMode = TraversalMode.InOrder;

        /// <summary>
        /// Gets or sets the root of the tree (the top-most node)
        /// </summary>
        public virtual Node<T> Root
        {
            get { return head; }
            set { head = value; }
        }

        /// <summary>
        /// Gets whether the tree is read-only
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the number of elements stored in the tree
        /// </summary>
        public virtual int Count
        {
            get { return size; }
        }

        /// <summary>
        /// Gets or sets the traversal mode of the tree
        /// </summary>
        public virtual TraversalMode TraversalOrder
        {
            get { return traversalMode; }
            set { traversalMode = value; }
        }

        /// <summary>
        /// Creates a new instance of a Binary Tree
        /// </summary>
        public BinaryTree()
        {
            head = null;
            size = 0;
        }

        /// <summary>
        /// Adds a new element to the tree
        /// </summary>
        public virtual void Add(T value)
        {
            Node<T> node = new Node<T>(value);
            this.Add(node);
        }

        /// <summary>
        /// Adds a node to the tree
        /// </summary>
        public virtual void Add(Node<T> node)
        {
            if (this.head == null) //first element being added
            {
                this.head = node; //set node as root of the tree
                node.Tree = this;
                size++;
            }
            else
            {
                if (node.Parent == null)
                    node.Parent = head; //start at head

                //Node is inserted on the left side if it is smaller or equal to the parent
                bool insertLeftSide = comparer((IComparable)node.Value, (IComparable)node.Parent.Value) <= 0;

                if (insertLeftSide) //insert on the left
                {
                    if (node.Parent.LeftChild == null)
                    {
                        node.Parent.LeftChild = node; //insert in left
                        size++;
                        node.Tree = this; //assign node to this binary tree
                    }
                    else
                    {
                        node.Parent = node.Parent.LeftChild; //scan down to left child
                        this.Add(node); //recursive call
                    }
                }
                else //insert on the right
                {
                    if (node.Parent.RightChild == null)
                    {
                        node.Parent.RightChild = node; //insert in right
                        size++;
                        node.Tree = this; //assign node to this binary tree
                    }
                    else
                    {
                        node.Parent = node.Parent.RightChild;
                        this.Add(node);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the first node in the tree with the parameter value.
        /// </summary>
        public virtual Node<T> Find(T value)
        {
            Node<T> node = this.head; //start at head
            while (node != null)
            {
                if (node.Value.Equals(value)) //parameter value found
                    return node;
                else
                {
                    //Search left if the value is smaller than the current node
                    bool searchLeft = comparer((IComparable)value, (IComparable)node.Value) < 0;

                    if (searchLeft)
                        node = node.LeftChild; //search left
                    else
                        node = node.RightChild; //search right
                }
            }

            return null; //not found
        }

        /// <summary>
        /// Returns whether a value is stored in the tree
        /// </summary>
        public virtual bool Contains(T value)
        {
            return (this.Find(value) != null);
        }

        /// <summary>
        /// Removes a value from the tree and returns whether the removal was successful.
        /// </summary>
        public virtual bool Remove(T value)
        {
            Node<T> removeNode = Find(value);

            return this.Remove(removeNode);
        }

        /// <summary>
        /// Removes a node from the tree and returns whether the removal was successful.
        /// </summary>>
        public virtual bool Remove(Node<T> removeNode)
        {
            if (removeNode == null || removeNode.Tree != this)
                return false; //value doesn't exist or not of this tree

            //Note whether the node to be removed is the root of the tree
            bool wasHead = (removeNode == head);

            if (this.Count == 1)
            {
                this.head = null; //only element was the root
                removeNode.Tree = null;

                size--; //decrease total element count
            }
            else if (removeNode.IsLeaf) //Case 1: No Children
            {
                //Remove node from its parent
                if (removeNode.IsLeftChild)
                    removeNode.Parent.LeftChild = null;
                else
                    removeNode.Parent.RightChild = null;

                removeNode.Tree = null;
                removeNode.Parent = null;

                size--; //decrease total element count
            }
            else if (removeNode.ChildCount == 1) //Case 2: One Child
            {
                if (removeNode.HasLeftChild)
                {
                    //Put left child node in place of the node to be removed
                    removeNode.LeftChild.Parent = removeNode.Parent; //update parent

                    if (wasHead)
                        this.Root = removeNode.LeftChild; //update root reference if needed

                    if (removeNode.IsLeftChild) //update the parent's child reference
                        removeNode.Parent.LeftChild = removeNode.LeftChild;
                    else
                        removeNode.Parent.RightChild = removeNode.LeftChild;
                }
                else //Has right child
                {
                    //Put left node in place of the node to be removed
                    removeNode.RightChild.Parent = removeNode.Parent; //update parent

                    if (wasHead)
                        this.Root = removeNode.RightChild; //update root reference if needed

                    if (removeNode.IsLeftChild) //update the parent's child reference
                        removeNode.Parent.LeftChild = removeNode.RightChild;
                    else
                        removeNode.Parent.RightChild = removeNode.RightChild;
                }

                removeNode.Tree = null;
                removeNode.Parent = null;
                removeNode.LeftChild = null;
                removeNode.RightChild = null;

                size--; //decrease total element count
            }
            else //Case 3: Two Children
            {
                //Find inorder predecessor (right-most node in left subtree)
                Node<T> successorNode = removeNode.LeftChild;
                while (successorNode.RightChild != null)
                {
                    successorNode = successorNode.RightChild;
                }

                removeNode.Value = successorNode.Value; //replace value

                this.Remove(successorNode); //recursively remove the inorder predecessor
            }


            return true;
        }

        /// <summary>
        /// Removes all the elements stored in the tree
        /// </summary>
        public virtual void Clear()
        {
            //Remove children first, then parent
            //(Post-order traversal)
            IEnumerator<T> enumerator = GetPostOrderEnumerator();
            while (enumerator.MoveNext())
            {
                this.Remove(enumerator.Current);
            }
            enumerator.Dispose();
        }

        /// <summary>
        /// Returns the height of the entire tree
        /// </summary>
        public virtual int GetHeight()
        {
            return this.GetHeight(this.Root);
        }

        /// <summary>
        /// Returns the height of the subtree rooted at the parameter value
        /// </summary>
        public virtual int GetHeight(T value)
        {
            //Find the value's node in tree
            Node<T> valueNode = this.Find(value);
            if (value != null)
                return this.GetHeight(valueNode);
            else
                return 0;
        }

        /// <summary>
        /// Returns the height of the subtree rooted at the parameter node
        /// </summary>
        public virtual int GetHeight(Node<T> startNode)
        {
            if (startNode == null)
                return 0;
            else
                return 1 + Math.Max(GetHeight(startNode.LeftChild), GetHeight(startNode.RightChild));
        }

        /// <summary>
        /// Returns the depth of a subtree rooted at the parameter value
        /// </summary>
        public virtual int GetDepth(T value)
        {
            Node<T> node = this.Find(value);
            return this.GetDepth(node);
        }

        /// <summary>
        /// Returns the depth of a subtree rooted at the parameter node
        /// </summary>
        public virtual int GetDepth(Node<T> startNode)
        {
            int depth = 0;

            if (startNode == null)
                return depth;

            Node<T> parentNode = startNode.Parent; //start a node above
            while (parentNode != null)
            {
                depth++;
                parentNode = parentNode.Parent; //scan up towards the root
            }

            return depth;
        }

        /// <summary>
        /// Returns an enumerator to scan through the elements stored in tree.
        /// The enumerator uses the traversal set in the TraversalMode property.
        /// </summary>
        public virtual IEnumerator<T> GetEnumerator()
        {
            switch (this.TraversalOrder)
            {
                case TraversalMode.InOrder:
                    return GetInOrderEnumerator();
                case TraversalMode.PostOrder:
                    return GetPostOrderEnumerator();
                case TraversalMode.PreOrder:
                    return GetPreOrderEnumerator();
                default:
                    return GetInOrderEnumerator();
            }
        }

        /// <summary>
        /// Returns an enumerator to scan through the elements stored in tree.
        /// The enumerator uses the traversal set in the TraversalMode property.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that visits node in the order: left child, parent, right child
        /// </summary>
        public virtual IEnumerator<T> GetInOrderEnumerator()
        {
            return new BinaryTreeInOrderEnumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that visits node in the order: left child, right child, parent
        /// </summary>
        public virtual IEnumerator<T> GetPostOrderEnumerator()
        {
            return new BinaryTreePostOrderEnumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that visits node in the order: parent, left child, right child
        /// </summary>
        public virtual IEnumerator<T> GetPreOrderEnumerator()
        {
            return new BinaryTreePreOrderEnumerator(this);
        }

        /// <summary>
        /// Copies the elements in the tree to an array using the traversal mode specified.
        /// </summary>
        public virtual void CopyTo(T[] array)
        {
            this.CopyTo(array, 0);
        }

        /// <summary>
        /// Copies the elements in the tree to an array using the traversal mode specified.
        /// </summary>
        public virtual void CopyTo(T[] array, int startIndex)
        {
            IEnumerator<T> enumerator = this.GetEnumerator();

            for (int i = startIndex; i < array.Length; i++)
            {
                if (enumerator.MoveNext())
                    array[i] = enumerator.Current;
                else
                    break;
            }
        }

        /// <summary>
        /// Compares two elements to determine their positions within the tree.
        /// </summary>
        public static int CompareElements(IComparable x, IComparable y)
        {
            return x.CompareTo(y);
        }

        /// <summary>
        /// Returns an inorder-traversal enumerator for the tree values
        /// </summary>
        internal class BinaryTreeInOrderEnumerator : IEnumerator<T>
        {
            private Node<T> current;
            private BinaryTree<T> tree;
            internal Queue<Node<T>> traverseQueue;

            public BinaryTreeInOrderEnumerator(BinaryTree<T> tree)
            {
                this.tree = tree;

                //Build queue
                traverseQueue = new Queue<Node<T>>();
                visitNode(this.tree.Root);
            }

            private void visitNode(Node<T> node)
            {
                if (node == null)
                    return;
                else
                {
                    visitNode(node.LeftChild);
                    traverseQueue.Enqueue(node);
                    visitNode(node.RightChild);
                }
            }

            public T Current
            {
                get { return current.Value; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                current = null;
                tree = null;
            }

            public void Reset()
            {
                current = null;
            }

            public bool MoveNext()
            {
                if (traverseQueue.Count > 0)
                    current = traverseQueue.Dequeue();
                else
                    current = null;

                return (current != null);
            }
        }

        /// <summary>
        /// Returns a postorder-traversal enumerator for the tree values
        /// </summary>
        internal class BinaryTreePostOrderEnumerator : IEnumerator<T>
        {
            private Node<T> current;
            private BinaryTree<T> tree;
            internal Queue<Node<T>> traverseQueue;

            public BinaryTreePostOrderEnumerator(BinaryTree<T> tree)
            {
                this.tree = tree;

                //Build queue
                traverseQueue = new Queue<Node<T>>();
                visitNode(this.tree.Root);
            }

            private void visitNode(Node<T> node)
            {
                if (node == null)
                    return;
                else
                {
                    visitNode(node.LeftChild);
                    visitNode(node.RightChild);
                    traverseQueue.Enqueue(node);
                }
            }

            public T Current
            {
                get { return current.Value; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                current = null;
                tree = null;
            }

            public void Reset()
            {
                current = null;
            }

            public bool MoveNext()
            {
                if (traverseQueue.Count > 0)
                    current = traverseQueue.Dequeue();
                else
                    current = null;

                return (current != null);
            }
        }

        /// <summary>
        /// Returns an preorder-traversal enumerator for the tree values
        /// </summary>
        internal class BinaryTreePreOrderEnumerator : IEnumerator<T>
        {
            private Node<T> current;
            private BinaryTree<T> tree;
            internal Queue<Node<T>> traverseQueue;

            public BinaryTreePreOrderEnumerator(BinaryTree<T> tree)
            {
                this.tree = tree;

                //Build queue
                traverseQueue = new Queue<Node<T>>();
                visitNode(this.tree.Root);
            }

            private void visitNode(Node<T> node)
            {
                if (node == null)
                    return;
                else
                {
                    traverseQueue.Enqueue(node);
                    visitNode(node.LeftChild);
                    visitNode(node.RightChild);
                }
            }

            public T Current
            {
                get { return current.Value; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                current = null;
                tree = null;
            }

            public void Reset()
            {
                current = null;
            }

            public bool MoveNext()
            {
                if (traverseQueue.Count > 0)
                    current = traverseQueue.Dequeue();
                else
                    current = null;

                return (current != null);
            }
        }
    }

    class AVLNode<T> : Node<T>
        where T : IComparable
    {
        public AVLNode(T value)
        : base(value)
        {
        }

        public new AVLNode<T> LeftChild
        {
            get
            {
                return (AVLNode<T>)base.LeftChild;
            }
            set
            {
                base.LeftChild = value;
            }
        }

        public new AVLNode<T> RightChild
        {
            get
            {
                return (AVLNode<T>)base.RightChild;
            }
            set
            {
                base.RightChild = value;
            }
        }

        public new AVLNode<T> Parent
        {
            get
            {
                return (AVLNode<T>)base.Parent;
            }
            set
            {
                base.Parent = value;
            }
        }
    }

    /// <summary>
    /// AVL Tree data structure
    /// </summary>
    class AVLTree<T> : BinaryTree<T>
        where T : IComparable
    {
        /// <summary>
        /// Returns the AVL Node of the tree
        /// </summary>
        public new AVLNode<T> Root
        {
            get
            {
                return (AVLNode<T>)base.Root;
            }
            set
            {
                base.Root = value;
            }
        }

        /// <summary>
        /// Returns the AVL Node corresponding to the given value
        /// </summary>
        public new AVLNode<T> Find(T value)
        {
            return (AVLNode<T>)base.Find(value);
        }

        /// <summary>
        /// Insert a value in the tree and rebalance the tree if necessary.
        /// </summary>
        public override void Add(T value)
        {
            AVLNode<T> node = new AVLNode<T>(value);

            base.Add(node); //add normally

            //Balance every node going up, starting with the parent
            AVLNode<T> parentNode = node.Parent;

            while (parentNode != null)
            {
                int balance = this.getBalance(parentNode);
                if (Math.Abs(balance) == 2) //-2 or 2 is unbalanced
                {
                    //Rebalance tree
                    this.balanceAt(parentNode, balance);
                }

                parentNode = parentNode.Parent; //keep going up
            }
        }

        /// <summary>
        /// Removes a given value from the tree and rebalances the tree if necessary.
        /// </summary>
        public override bool Remove(T value)
        {
            AVLNode<T> valueNode = this.Find(value);
            return this.Remove(valueNode);
        }

        /// <summary>
        /// Wrapper method for removing a node within the tree
        /// </summary>
        protected new bool Remove(Node<T> removeNode)
        {
            return this.Remove((AVLNode<T>)removeNode);
        }

        /// <summary>
        /// Removes a given node from the tree and rebalances the tree if necessary.
        /// </summary>
        public bool Remove(AVLNode<T> valueNode)
        {
            //Save reference to the parent node to be removed
            AVLNode<T> parentNode = valueNode.Parent;

            //Remove the node as usual
            bool removed = base.Remove(valueNode);

            if (!removed)
            {
                return false;    //removing failed, no need to rebalance
            }
            else
            {
                //Balance going up the tree
                while (parentNode != null)
                {
                    int balance = this.getBalance(parentNode);

                    if (Math.Abs(balance) == 1) //1, -1
                    {
                        break;    //height hasn't changed, can stop
                    }
                    else if (Math.Abs(balance) == 2) //2, -2
                    {
                        //Rebalance tree
                        this.balanceAt(parentNode, balance);
                    }

                    parentNode = parentNode.Parent;
                }

                return true;
            }
        }

        /// <summary>
        /// Balances an AVL Tree node
        /// </summary>
        protected virtual void balanceAt(AVLNode<T> node, int balance)
        {
            if (balance == 2) //right outweighs
            {
                int rightBalance = getBalance(node.RightChild);

                if (rightBalance == 1 || rightBalance == 0)
                {
                    //Left rotation needed
                    rotateLeft(node);
                }
                else if (rightBalance == -1)
                {
                    //Right rotation needed
                    rotateRight(node.RightChild);

                    //Left rotation needed
                    rotateLeft(node);
                }
            }
            else if (balance == -2) //left outweighs
            {
                int leftBalance = getBalance(node.LeftChild);
                if (leftBalance == 1)
                {
                    //Left rotation needed
                    rotateLeft(node.LeftChild);

                    //Right rotation needed
                    rotateRight(node);
                }
                else if (leftBalance == -1 || leftBalance == 0)
                {
                    //Right rotation needed
                    rotateRight(node);
                }
            }
        }

        /// <summary>
        /// Determines the balance of a given node
        /// </summary>
        protected virtual int getBalance(AVLNode<T> root)
        {
            //Balance = right child's height - left child's height
            return this.GetHeight(root.RightChild) - this.GetHeight(root.LeftChild);
        }

        /// <summary>
        /// Rotates a node to the left within an AVL Tree
        /// </summary>
        protected virtual void rotateLeft(AVLNode<T> root)
        {
            if (root == null)
            {
                return;
            }

            AVLNode<T> pivot = root.RightChild;

            if (pivot == null)
            {
                return;
            }
            else
            {
                AVLNode<T> rootParent = root.Parent; //original parent of root node
                bool isLeftChild = (rootParent != null) && rootParent.LeftChild == root; //whether the root was the parent's left node
                bool makeTreeRoot = root.Tree.Root == root; //whether the root was the root of the entire tree

                //Rotate
                root.RightChild = pivot.LeftChild;
                pivot.LeftChild = root;

                //Update parents
                root.Parent = pivot;
                pivot.Parent = rootParent;

                if (root.RightChild != null)
                {
                    root.RightChild.Parent = root;
                }

                //Update the entire tree's Root if necessary
                if (makeTreeRoot)
                {
                    pivot.Tree.Root = pivot;
                }

                //Update the original parent's child node
                if (isLeftChild)
                {
                    rootParent.LeftChild = pivot;
                }
                else if (rootParent != null)
                {
                    rootParent.RightChild = pivot;
                }
            }
        }

        /// <summary>
        /// Rotates a node to the right within an AVL Tree
        /// </summary>
        protected virtual void rotateRight(AVLNode<T> root)
        {
            if (root == null)
            {
                return;
            }

            AVLNode<T> pivot = root.LeftChild;

            if (pivot == null)
            {
                return;
            }
            else
            {
                AVLNode<T> rootParent = root.Parent; //original parent of root node
                bool isLeftChild = (rootParent != null) && rootParent.LeftChild == root; //whether the root was the parent's left node
                bool makeTreeRoot = root.Tree.Root == root; //whether the root was the root of the entire tree

                //Rotate
                root.LeftChild = pivot.RightChild;
                pivot.RightChild = root;

                //Update parents
                root.Parent = pivot;
                pivot.Parent = rootParent;

                if (root.LeftChild != null)
                {
                    root.LeftChild.Parent = root;
                }

                //Update the entire tree's Root if necessary
                if (makeTreeRoot)
                {
                    pivot.Tree.Root = pivot;
                }

                //Update the original parent's child node
                if (isLeftChild)
                {
                    rootParent.LeftChild = pivot;
                }
                else if (rootParent != null)
                {
                    rootParent.RightChild = pivot;
                }
            }
        }
    }
}

