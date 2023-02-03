import networkx as nx
import random
import matplotlib.pyplot as plt
import pickle

'''G = nx.random_tree(20)
nodes = list(G.nodes())
for x in range(10):
    edge = random.choices(nodes, k=2)
    if edge[0] != edge[1]:
        G.add_edge(edge[0], edge[1])
while not nx.is_planar(G):
    G = nx.random_tree(20)
    nodes = list(G.nodes())
    for x in range(10):
        edge = random.choices(nodes, k=2)
        if edge[0] != edge[1]:
            G.add_edge(edge[0], edge[1])
A = nx.adjacency_matrix(G)
A = A.todense()

with open('fixed_example.pkl', 'wb') as f:
    pickle.dump(A, f)'''

with open('fixed_example.pkl', 'rb') as f:
    A = pickle.load(f)


G = nx.from_numpy_array(A)
nx.draw(G)
plt.show()