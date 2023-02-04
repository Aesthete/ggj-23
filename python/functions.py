import numpy as np
import networkx as nx
import random
from typing import Union
import copy
from itertools import combinations

def is_crossing(point1: list, point2: list, point3: list, point4: list, p: bool = False) -> bool:
    if (point1[0] - point2[0]) == 0:
        x = point1[0]
        if (point3[0] < x and point4[0] > x) or (point3[0] < x and point4[0] > x):
            return True
    if (point3[0] - point4[0]) == 0:
        x = point3[0]
        if (point1[0] < x and point2[0] > x) or (point2[0] < x and point1[0] > x):
            return True

    a = (point1[1] - point2[1])/(point1[0] - point2[0])
    b = (point3[1] - point4[1]) / (point3[0] - point4[0])
    x = (a*point1[0] - point1[1] - b*point3[0] + point3[1])/(a-b)
    y = b*(x - point3[0]) + point3[1]
    if p:
        print([a, b, x, y])
    if (point1[0] < x and point2[0] >x) or (point2[0] < x and point1[0] >x):
        if (point1[1] < y and point2[1] >y) or (point2[1] < y and point1[1] >y):
            if (point3[0] < x and point4[0] > x) or (point4[0] < x and point3[0] > x):
                if (point3[1] < y and point4[1] > y) or (point4[1] < y and point3[1] > y):
                    return True
    return False

def pos_max(pos: dict) -> (float,float):
    max_x = 0
    max_y = 0
    for k in pos.keys():
        if pos[k][0]>max_x:
            max_x = pos[k][0]
        if pos[k][1]>max_y:
            max_y = pos[k][1]
    return max_x, max_y

def pos_scaled(pos: dict) -> dict:
    max_x, max_y = pos_max(pos)
    for k in pos.keys():
        pos[k] = (pos[k][0]/max_x, pos[k][1]/max_y)
    return pos

def make_planar_treelike(n: int, m: int, attempts: int=100) -> Union[bool,np.ndarray]:
    G = nx.random_tree(n)
    nodes = list(G.nodes())
    i=1
    for x in range(attempts):
        if i<m:
            edge = random.choices(nodes, k=2)
            if edge[0] != edge[1]:
                G.add_edge(edge[0], edge[1])
                if not nx.is_planar(G):
                    G.remove_edge(edge[0], edge[1])
                else:
                    i+=1
        else:
            break
    #i = 1
    #while not nx.is_planar(G) and i<attempts:
    #    G = nx.random_tree(n)
    #    nodes = list(G.nodes())
    #    for x in range(m):
    #        edge = random.choices(nodes, k=2)
    #        if edge[0] != edge[1]:
    #            G.add_edge(edge[0], edge[1])
    #    i+=1
    #if i==attempts:
    #    return False
    A = nx.adjacency_matrix(G)
    A = A.todense()
    return A

def planarizing_embedding_from_matrix(A: np.ndarray) -> (list, dict):
    G = nx.from_numpy_array(A)
    pos = nx.nx_agraph.graphviz_layout(G, prog='neato', root=None, args='')

    edges = list(G.edges())
    x = 1
    l = len(edges)
    edges_copy = copy.deepcopy(edges)
    edge_pairs = combinations(edges, 2)
    for ep in edge_pairs:
        e1 = ep[0]
        e2 = ep[1]
        total_nodes = list(set(list(e1) + list(e2)))
        if len(total_nodes) == 4:
            if is_crossing(pos[e1[0]], pos[e1[1]], pos[e2[0]], pos[e2[1]]):
                try:
                    edges_copy.remove(e1)
                except:
                    continue
    return edges_copy, pos

def largest_planar_component(edges: list, pos: dict) -> nx.Graph:
    G = nx.Graph()
    for e in range(len(pos)):
        G.add_node(e)
    for e in edges:
        G.add_edge(e[0], e[1])

    largest_cc = max(nx.connected_components(G), key=len)
    new_pos = {}
    for lc in largest_cc:
        new_pos[lc] = pos[lc]
    G = nx.Graph()
    for e in largest_cc:
        G.add_node(e)
    for e in edges:
        if e[0] in largest_cc and e[1] in largest_cc:
            G.add_edge(e[0], e[1])
    return G