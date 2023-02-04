from functions import *
import matplotlib.pyplot as plt

A = make_planar_treelike(100,15)

if isinstance(A,np.ndarray):
    edges, pos = planarizing_embedding_from_matrix(A)

    G = largest_planar_component(edges, pos)
else:
    G = nx.Graph()
    pos = []

nx.draw(G, pos=pos)
plt.show()

